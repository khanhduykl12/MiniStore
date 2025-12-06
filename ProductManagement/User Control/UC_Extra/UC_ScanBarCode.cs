using AForge.Video.DirectShow;
using MiniStore.Models;
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
            try
            {
                Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

   
                if (picCamera.InvokeRequired)
                {
                    picCamera.BeginInvoke(new Action(() =>
                    {
                        picCamera.Image?.Dispose();               
                        picCamera.Image = (Bitmap)frame.Clone(); 
                    }));
                }
                else
                {
                    picCamera.Image?.Dispose();
                    picCamera.Image = (Bitmap)frame.Clone();
                }

                if (_decoded) return;

                // Đọc barcode
                var reader = new BarcodeReader();  
                var result = reader.Decode(frame);
                if (result != null)
                {
                    _decoded = true;
                    _lastBarcode = result.Text;

                    txtBarCode.Invoke(new MethodInvoker(() =>
                    {
                        txtBarCode.Text = _lastBarcode;
                    }));
                }
            }
            catch
            {
                
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
                _videoDevice.SignalToStop();
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

            // tìm sản phẩm theo BARCODE
            var sp = db.SANPHAMs.FirstOrDefault(x => x.BARCODE == _lastBarcode);
            if (sp == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm với barcode này.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Bắn event cho Form cha (ShoppingCartStaff)
            ProductScanned?.Invoke(this, new ProductScannedEventArgs
            {
                MaSP = sp.MASP,
                TenSP = sp.TENSP,
                GiaBan = sp.GIABAN ?? 0,
                DVT = sp.DVT,
                Hinh = sp.HINH
            });

            // Sau khi lưu xong có thể clear & cho quét tiếp
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
