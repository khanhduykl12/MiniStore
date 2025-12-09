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
using ZXing;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace MiniShop.User_Control.UC_Extra
{

    public partial class UC_ScanBarCode : UserControl
    {
        private FilterInfoCollection _cameras;
        private VideoCaptureDevice _videoDevice;
        private bool _decoded = false;
        private string _lastBarcode = string.Empty;
        private readonly MiniStoreContext db = new MiniStoreContext();

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
            _videoDevice = new VideoCaptureDevice(_cameras[cboCamera.SelectedIndex].MonikerString);
            _videoDevice.NewFrame += VideoDevice_NewFrame;
            _videoDevice.Start();
            _decoded = false;
            _lastBarcode = string.Empty;
            txtBarCode.Text = "";
        }

        private void VideoDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap frameForDecode = null;
            try
            {
                // Clone incoming buffer once to own a safe copy
                frameForDecode = (Bitmap)eventArgs.Frame.Clone();

                // Create a separate bitmap instance for display (so decode and UI use independent bitmaps)
                Bitmap displayBmp = new Bitmap(frameForDecode);

                // Safely update UI picture box with the display copy
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
                            // If assignment fails, dispose the display bitmap to avoid leak
                            try { displayBmp.Dispose(); } catch { /* ignore */ }
                        }
                    }));
                }
                else
                {
                    picCamera.Image?.Dispose();
                    picCamera.Image = displayBmp;
                }

                // If already decoded, skip decoding
                if (_decoded) return;

                // Decode barcode from our decode-safe clone
                var reader = new BarcodeReader();
                var result = reader.Decode(frameForDecode);
                if (result != null)
                {
                    _decoded = true;
                    _lastBarcode = result.Text;

                    // update text box on UI thread
                    if (txtBarCode.InvokeRequired)
                    {
                        txtBarCode.BeginInvoke(new MethodInvoker(() =>
                        {
                            txtBarCode.Text = _lastBarcode;
                        }));
                    }
                    else
                    {
                        txtBarCode.Text = _lastBarcode;
                    }
                }
            }
            catch
            {
                // swallow frame errors to keep scanner running
            }
            finally
            {
                // Always free the decode clone; UI owns displayBmp so don't touch it here
                frameForDecode?.Dispose();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopCamera();
        }

        private void StopCamera()
        {
            if (_videoDevice != null && _videoDevice.IsRunning)
            {
                try
                {
                    _videoDevice.SignalToStop();
                    _videoDevice.WaitForStop();
                }
                catch
                {
                    // ignore stop errors
                }
                _videoDevice.NewFrame -= VideoDevice_NewFrame;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_lastBarcode))
            {
                MessageBox.Show("Chưa quét được barcode nào.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // tìm sản phẩm theo BARCODE, kèm HANGTRUNGBAY/SANPHAM để biết tồn
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

            if (sp == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm với barcode này.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // allow next scans
                _decoded = false;
                _lastBarcode = string.Empty;
                txtBarCode.Text = "";
                return;
            }

            // determine available on shelf (prefer HANGTRUNGBAY), fallback to SANPHAM.SOLUONG
            int available = sp.SoLuongTrenKe > 0 ? sp.SoLuongTrenKe : (int)(sp.TonKho);

            // consider current quantity already in cart
            int currentlyInCart = CartService.Items.FirstOrDefault(i => i.MaSP == sp.MASP)?.SoLuong ?? 0;

            if (available - currentlyInCart <= 0)
            {
                MessageBox.Show($"Không đủ hàng trên kệ để thêm sản phẩm \"{sp.TENSP}\". Số lượng trên kệ: {available}, đã có trong giỏ: {currentlyInCart}",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // allow next scans
                _decoded = false;
                _lastBarcode = string.Empty;
                txtBarCode.Text = "";
                return;
            }

            // Bắn event cho Form cha (ShoppingCartStaff) với thông tin sản phẩm
            ProductScanned?.Invoke(this, new ProductScannedEventArgs
            {
                MaSP = sp.MASP,
                TenSP = sp.TENSP,
                GiaBan = sp.GiaBan,
                DVT = sp.DVT,
                Hinh = sp.HINH
            });

            // Sau khi lưu xong cho phép quét tiếp
            _decoded = false;
            _lastBarcode = string.Empty;
            txtBarCode.Text = "";
        }

        public void CloseScanner()
        {
            StopCamera();
            _decoded = false;
            _lastBarcode = string.Empty;
            txtBarCode.Text = "";
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            StopCamera();
            db.Dispose();
            base.OnHandleDestroyed(e);
        }
    }
}
