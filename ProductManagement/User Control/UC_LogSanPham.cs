using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace MiniShop.User_Control
{
    public partial class UC_LogThemSanPham : UserControl
    {
        public UC_LogThemSanPham()
        {
            InitializeComponent();
            flpLogEntries.Resize += FlpLogEntries_Resize;
        }

        public class LogEntry
        {
            public string EmployeeName { get; set; } = string.Empty;
            public string ProductName { get; set; } = string.Empty;
            public string ProductCode { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public string SupplierName { get; set; } = string.Empty;
            public DateTime AddedAt { get; set; }
            public string Status { get; set; } = string.Empty; // "Thêm" hoặc "Xóa"
        }

        public void SetLogs(IEnumerable<LogEntry> entries)
        {
            flpLogEntries.SuspendLayout();
            flpLogEntries.Controls.Clear();

            var list = entries?.ToList() ?? new List<LogEntry>();

            if (list.Count == 0)
            {
                // Empty state
                flpLogEntries.Controls.Add(CreateEmptyPanel());
            }
            else
            {
                foreach (var entry in list)
                {
                    flpLogEntries.Controls.Add(CreateLogPanel(entry));
                }
            }

            AdjustPanelWidths();
            flpLogEntries.ResumeLayout();
        }

        private void FlpLogEntries_Resize(object sender, EventArgs e)
        {
            AdjustPanelWidths();
        }

        private void AdjustPanelWidths()
        {
            var width = CalculateEntryWidth();
            foreach (Panel panel in flpLogEntries.Controls.OfType<Panel>())
            {
                panel.Width = width;
            }
        }

        private int CalculateEntryWidth()
        {
            var available = flpLogEntries.ClientSize.Width - flpLogEntries.Padding.Horizontal;
            return Math.Max(available - 20, 320);
        }

        private Panel CreateLogPanel(LogEntry entry)
        {
            var container = new Panel
            {
                BackColor = Color.White,
                Margin = new Padding(6),
                Padding = new Padding(12),
                Width = CalculateEntryWidth(),
                MinimumSize = new Size(320, 150),
                BorderStyle = BorderStyle.FixedSingle
            };

            // =====================================================================
            // HEADER (Tên sản phẩm)
            // =====================================================================
            var header = new TableLayoutPanel
            {
                Location = new Point(0, 0),
                Width = container.Width - 20,
                ColumnCount = 2,
                AutoSize = false,
                Height = 28
            };

            header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            header.Controls.Add(new Label
            {
                AutoSize = false,
                Width = 150,
                Text = "Tên sản phẩm:",
                Margin = new Padding(0, 4, 6, 4)
            }, 0, 0);

            header.Controls.Add(new Label
            {
                AutoSize = false,
                Width = header.Width - 150,
                Text = entry.ProductName,
                Font = new Font(Font, FontStyle.Bold),
                Margin = new Padding(0, 4, 0, 4)
            }, 1, 0);

            container.Controls.Add(header);

            // =====================================================================
            // MAIN TABLE (Gồm: mã – số lượng – giá – nhân viên)
            // =====================================================================

            var layout = new TableLayoutPanel
            {
                Location = new Point(0, header.Bottom + 6),
                Width = container.Width - 20,
                ColumnCount = 2,
                AutoSize = false,
                Height = 170
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150)); // KEY COLUMN WIDTH
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));  // VALUE COLUMN

            void AddInfo(string key, string value, int extraBottom = 0)
            {
                int row = layout.RowCount++;
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                layout.Controls.Add(new Label
                {
                    AutoSize = false,
                    Width = 150,
                    Text = key,
                    Margin = new Padding(0, 4, 6, 4 + extraBottom) // padding dưới
                }, 0, row);

                layout.Controls.Add(new Label
                {
                    AutoSize = false,
                    Width = layout.Width - 150,
                    Text = value,
                    Font = new Font(Font, FontStyle.Bold),
                    Margin = new Padding(0, 4, 0, 4 + extraBottom)
                }, 1, row);
            }


            // ======== THEO ĐÚNG THỨ TỰ BẠN MUỐN ========
            AddInfo("Mã sản phẩm:", entry.ProductCode);
            AddInfo("Số lượng:", entry.Quantity.ToString());
            AddInfo("Giá:", FormatMoney(entry.Price));
            AddInfo("Nhân viên:", entry.EmployeeName, extraBottom: 12);

            container.Controls.Add(layout);

            // =====================================================================
            // BADGE TRẠNG THÁI (Thêm | Xóa | Cập nhật)
            // =====================================================================

            var lblStatusBadge = new Label
            {
                AutoSize = true,
                Font = new Font(Font, FontStyle.Bold),
                ForeColor = Color.White,
                Padding = new Padding(10, 4, 10, 4),
                Text = entry.Status,
                BackColor = GetStatusColor(entry.Status)
            };
            container.Controls.Add(lblStatusBadge);
            lblStatusBadge.BringToFront();

            // =====================================================================
            // LABEL THỜI GIAN — thẳng cột với badge
            // =====================================================================

            var lblTime = new Label
            {
                AutoSize = true,
                ForeColor = Color.DimGray,
                Font = new Font(Font, FontStyle.Regular),
                Text = entry.AddedAt.ToString("HH:mm dd/MM/yyyy")
            };

            container.Controls.Add(lblTime);
            lblTime.BringToFront();

            // =====================================================================
            // POSITIONING — badge + time
            // =====================================================================

            void UpdatePositions()
            {
                lblStatusBadge.Location = new Point(
                    container.Width - lblStatusBadge.Width - 12,
                    10
                );

                lblTime.Location = new Point(
                    container.Width - lblTime.Width - 12,
                    lblStatusBadge.Bottom + 6
                );
            }

            UpdatePositions();
            container.Resize += (s, e) => UpdatePositions();
            lblStatusBadge.SizeChanged += (s, e) => UpdatePositions();
            lblTime.SizeChanged += (s, e) => UpdatePositions();

            return container;
        }

        private Color GetStatusColor(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return Color.DimGray;
            var normalized = status.Trim().ToLowerInvariant();
            return normalized switch
            {
                "hoàn tất" => Color.FromArgb(76, 175, 80),
                "đang xử lý" => Color.FromArgb(255, 152, 0),
                "chưa duyệt" => Color.FromArgb(244, 67, 54),
                "thêm" => Color.FromArgb(76, 175, 80),
                "xóa" => Color.FromArgb(244, 67, 54),
                "cập nhật" => Color.FromArgb(255, 152, 0),
                _ => Color.DimGray
            };
        }

        private Panel CreateEmptyPanel()
        {
            return new Panel
            {
                BackColor = Color.White,
                Margin = new Padding(6),
                Padding = new Padding(12),
                Width = CalculateEntryWidth(),
                MinimumSize = new Size(320, 120),
                BorderStyle = BorderStyle.FixedSingle,
                Controls =
                {
                    new Label
                    {
                        AutoSize = false,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font(Font, FontStyle.Italic),
                        ForeColor = Color.DimGray,
                        Text = "Chưa có nhật ký thêm sản phẩm."
                    }
                }
            };
        }

        private void AddRow(TableLayoutPanel layout, string label, string value)
        {
            var rowIndex = layout.RowCount++;
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var lblKey = new Label
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Font = new Font(Font, FontStyle.Regular),
                Text = label,
                Margin = new Padding(0, 2, 6, 2)
            };

            var lblValue = new Label
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Font = new Font(Font, FontStyle.Bold),
                Text = value,
                Margin = new Padding(0, 2, 0, 2)
            };

            layout.Controls.Add(lblKey, 0, rowIndex);
            layout.Controls.Add(lblValue, 1, rowIndex);
        }

        private string FormatMoney(decimal price)
        {
            return price.ToString("N0", CultureInfo.InvariantCulture);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var parent = this.Parent;

            if (Tag is Control previous && !previous.IsDisposed)
            {
                previous.Visible = true;
                previous.BringToFront();
            }

            if (parent != null)
            {
                parent.Controls.Remove(this);
            }

            Dispose();
        }
    }
}
