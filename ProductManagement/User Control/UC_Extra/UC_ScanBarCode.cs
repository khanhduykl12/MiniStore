using AForge.Video.DirectShow;
using MiniStore.Models;
using MiniStore.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ZXing;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;
using ZXing.Common;

namespace MiniShop.User_Control.UC_Extra
{

    public partial class UC_ScanBarCode : UserControl
    {
        public event Action<string> OnBarcodeScanned;
        private FilterInfoCollection _cameras;
        private VideoCaptureDevice _videoDevice;
        private volatile bool _decoded = false;
        private volatile int _handledFlag = 0; // 0 = not handled, 1 = handled
        private string _lastBarcode = string.Empty;
        private readonly MiniStoreContext db = new MiniStoreContext();

        // Skip initial frames after starting camera to avoid residual static frames
        private int _framesToSkip = 12; // increased to avoid stale frames
        private int _framesSeen = 0;

        public bool IsActive { get; private set; } = false;

        public class ProductScannedEventArgs : EventArgs
        {
            public string MaSP { get; set; }
            public string TenSP { get; set; }
            public decimal GiaBan { get; set; }
            public string? DVT { get; set; }
            public string? Hinh { get; set; }
        }

        public event EventHandler<ProductScannedEventArgs> ProductScanned;

        public UC_ScanBarCode()
        {
            InitializeComponent();
            this.Load += UC_ScanBarCode_Load;
            btnStart.Click += btnStart_Click;
            btnSave.Click += btnSave_Click;
            btnStop.Click += btnStop_Click;

            // start with save disabled until a barcode is detected
            btnSave.Enabled = false;
            // ensure product name textbox cleared if exists
            if (txtTenSP != null) txtTenSP.Text = string.Empty;
        }

        private void UC_ScanBarCode_Load(object sender, EventArgs e)
        {
            _cameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            cboCamera.Items.Clear();
            foreach (FilterInfo cam in _cameras)
            {
                cboCamera.Items.Add(cam.Name);
            }
            if (cboCamera.Items.Count > 0)
                cboCamera.SelectedIndex = 0;
            else
                MessageBox.Show("Không tìm thấy webcam.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cboCamera.SelectedIndex < 0) return;
            // ensure previous camera stopped and UI cleared
            StopCamera();

            // small delay for device to release
            try { Application.DoEvents(); Thread.Sleep(120); } catch { }

            // clear displayed image to avoid stale frame
            if (picCamera.Image != null)
            {
                try { picCamera.Image.Dispose(); } catch { }
                picCamera.Image = null;
            }
            picCamera.Refresh();

            _videoDevice = new VideoCaptureDevice(_cameras[cboCamera.SelectedIndex].MonikerString);
            // attach handler AFTER any previous device fully stopped
            _videoDevice.NewFrame += VideoDevice_NewFrame;
            _videoDevice.Start();
            _decoded = false;
            Interlocked.Exchange(ref _handledFlag, 0);
            _lastBarcode = string.Empty;
            txtBarCode.Text = "";
            // reset frame skipping
            _framesSeen = 0;
            // ensure save disabled until detection
            btnSave.Enabled = false;
            // clear product name field
            if (txtTenSP != null) txtTenSP.Text = string.Empty;
            IsActive = true;
        }

        private void VideoDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap frameForDisplay = null;
            try
            {
                frameForDisplay = (Bitmap)eventArgs.Frame.Clone();
                Bitmap displayBmp = new Bitmap(frameForDisplay);

                // Update UI image
                if (picCamera.InvokeRequired)
                {
                    picCamera.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            picCamera.Image?.Dispose();
                            picCamera.Image = displayBmp;
                        }
                        catch
                        {
                            try { displayBmp.Dispose(); } catch { }
                        }
                    }));
                }
                else
                {
                    picCamera.Image?.Dispose();
                    picCamera.Image = displayBmp;
                }

                // Skip initial frames to avoid decoding stale/previous images
                _framesSeen++;
                if (_framesSeen <= _framesToSkip)
                {
                    return;
                }

                if (_decoded) return;

                Bitmap decodeBmp = (Bitmap)frameForDisplay.Clone();
                Task.Run(() =>
                {
                    try
                    {
                        var options = new DecodingOptions
                        {
                            TryHarder = true,
                            TryInverted = true,
                            PossibleFormats = new List<BarcodeFormat>
                            {
                                BarcodeFormat.CODE_128,
                                BarcodeFormat.CODE_39,
                                BarcodeFormat.EAN_13,
                                BarcodeFormat.EAN_8,
                                BarcodeFormat.UPC_A,
                                BarcodeFormat.UPC_E,
                                BarcodeFormat.ITF,
                                BarcodeFormat.CODABAR,
                                BarcodeFormat.QR_CODE
                            }
                        };

                        var reader = new BarcodeReader()
                        {
                            AutoRotate = true,
                            Options = options
                        };

                        using (var bmpForDecode = new Bitmap(decodeBmp))
                        {
                            var result = reader.Decode(bmpForDecode);
                            if (result != null && !string.IsNullOrWhiteSpace(result.Text))
                            {
                                // ensure only one handler proceeds
                                if (Interlocked.Exchange(ref _handledFlag, 1) == 0)
                                {
                                    _decoded = true;
                                    _lastBarcode = result.Text;

                                    if (this.IsHandleCreated)
                                    {
                                        try
                                        {
                                            this.BeginInvoke(new Action(() =>
                                            {
                                                // show detected barcode but DO NOT auto-process until confirm
                                                txtBarCode.Text = _lastBarcode;

                                                // lookup product name and availability and set txtTenSP if available
                                                try
                                                {
                                                    var sp = db.SANPHAMs
                                                        .Where(x => x.BARCODE == _lastBarcode)
                                                        .Select(x => new
                                                        {
                                                            x.MASP,
                                                            x.TENSP,
                                                            GiaBan = x.GIABAN ?? 0m,
                                                            x.DVT,
                                                            x.HINH,
                                                            SoLuongTrenKe = x.HANGTRUNGBAY != null ? x.HANGTRUNGBAY.SOLUONG_TRENKE : 0,
                                                            TonKho = x.SOLUONG
                                                        })
                                                        .FirstOrDefault();

                                                    if (sp != null)
                                                    {
                                                        if (txtTenSP != null)
                                                            txtTenSP.Text = sp.TENSP ?? string.Empty;

                                                        int available = sp.SoLuongTrenKe > 0 ? sp.SoLuongTrenKe : (int)sp.TonKho;
                                                        int currentlyInCart = CartService.Items.FirstOrDefault(i => i.MaSP == sp.MASP)?.SoLuong ?? 0;

                                                        if (available - currentlyInCart <= 0)
                                                        {
                                                            // show out of shelf message and prevent confirming
                                                            MessageBox.Show($"Sản phẩm \"{sp.TENSP}\" đã hết trên kệ.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                            btnSave.Enabled = false;
                                                        }
                                                        else
                                                        {
                                                            btnSave.Enabled = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // product not found
                                                        if (txtTenSP != null) txtTenSP.Text = string.Empty;
                                                        btnSave.Enabled = false;
                                                    }
                                                }
                                                catch
                                                {
                                                    // ignore DB lookup errors but ensure save disabled to be safe
                                                    btnSave.Enabled = false;
                                                }

                                                // stop camera to freeze image but keep barcode for manual confirm
                                                try { StopCamera(); } catch { }
                                            }));
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        decodeBmp.Dispose();
                    }
                });
            }
            catch
            {
            }
            finally
            {
                frameForDisplay?.Dispose();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            CloseScanner();
        }

        private void StopCamera()
        {
            // detach handler first to prevent new frames from being enqueued into our handler
            try
            {
                if (_videoDevice != null)
                {
                    _videoDevice.NewFrame -= VideoDevice_NewFrame;
                }
            }
            catch { }

            if (_videoDevice != null && _videoDevice.IsRunning)
            {
                try
                {
                    _videoDevice.SignalToStop();
                    _videoDevice.WaitForStop();
                }
                catch
                {
                }
            }

            // ensure device reference cleared
            try { _videoDevice = null; } catch { }

            // clear displayed image to avoid stale frame causing re-detection on restart
            if (picCamera != null && picCamera.Image != null)
            {
                try { picCamera.Image.Dispose(); } catch { }
                picCamera.Image = null;
            }
            picCamera.Refresh();

            IsActive = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Manual save fallback: use displayed textbox value to avoid relying on _lastBarcode
            var barcode = txtBarCode.Text?.Trim();
            if (string.IsNullOrWhiteSpace(barcode))
            {
                // do nothing instead of showing message
                return;
            }

            // Attempt to process (force, even if IsActive false)
            ProcessBarcode(barcode, force: true);

            // disable save after processing to avoid accidental re-submit
            btnSave.Enabled = false;
        }

        private void ProcessBarcode(string barcode, bool force = false)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                Interlocked.Exchange(ref _handledFlag, 0);
                return;
            }

            // lookup product
            var sp = db.SANPHAMs
                .Where(x => x.BARCODE == barcode)
                .Select(x => new
                {
                    x.MASP,
                    x.TENSP,
                    GiaBan = x.GIABAN ?? 0m,
                    x.DVT,
                    x.HINH,
                    SoLuongTrenKe = x.HANGTRUNGBAY != null ? x.HANGTRUNGBAY.SOLUONG_TRENKE : 0,
                    TonKho = x.SOLUONG
                })
                .FirstOrDefault();

            if (sp == null)
            {
                // allow retry
                Interlocked.Exchange(ref _handledFlag, 0);
                MessageBox.Show("Barcode không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int available = sp.SoLuongTrenKe > 0 ? sp.SoLuongTrenKe : (int)(sp.TonKho);
            int currentlyInCart = CartService.Items.FirstOrDefault(i => i.MaSP == sp.MASP)?.SoLuong ?? 0;

            if (available - currentlyInCart <= 0)
            {
                Interlocked.Exchange(ref _handledFlag, 0);
                MessageBox.Show($"Sản phẩm \"{sp.TENSP}\" đã hết trên kệ.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // If scanner not active and not forced, ignore (prevents auto-handling after closed)
            if (!IsActive && !force)
            {
                Interlocked.Exchange(ref _handledFlag, 0);
                return;
            }

            // Raise event to parent to add product
            ProductScanned?.Invoke(this, new ProductScannedEventArgs
            {
                MaSP = sp.MASP,
                TenSP = sp.TENSP,
                GiaBan = sp.GiaBan,
                DVT = sp.DVT,
                Hinh = sp.HINH
            });

            // optionally notify barcode string listeners
            OnBarcodeScanned?.Invoke(barcode);

            // keep handled flag as 1 until CloseScanner resets it
        }

        public void CloseScanner()
        {
            StopCamera();
            _decoded = false;
            _lastBarcode = string.Empty;
            txtBarCode.Text = "";
            if (txtTenSP != null) txtTenSP.Text = string.Empty;
            Interlocked.Exchange(ref _handledFlag, 0);
            IsActive = false;

            // ensure save disabled
            btnSave.Enabled = false;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            StopCamera();
            db.Dispose();
            base.OnHandleDestroyed(e);
        }

        private void ShowResult(string barcode)
        {
            OnBarcodeScanned?.Invoke(barcode);
        }
    }
}
