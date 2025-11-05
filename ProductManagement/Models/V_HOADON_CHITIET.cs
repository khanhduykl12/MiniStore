using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Models;

[Keyless]
public partial class V_HOADON_CHITIET
{
    [StringLength(10)]
    [Unicode(false)]
    public string MAHD { get; set; } = null!;

    public DateOnly NGAYLAP { get; set; }

    public int NGUOILAP_ID { get; set; }

    [StringLength(30)]
    public string TenNguoiLap { get; set; } = null!;

    public int NGUOIMUA_ID { get; set; }

    [StringLength(30)]
    public string TenNguoiMua { get; set; } = null!;

    [StringLength(200)]
    public string? GHICHU { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string MASP { get; set; } = null!;

    [StringLength(100)]
    public string? TENSP { get; set; }

    public int? SOLUONG { get; set; }

    [Column(TypeName = "money")]
    public decimal? DONGIA { get; set; }

    [Column(TypeName = "money")]
    public decimal? THANHTIEN { get; set; }

    [Column(TypeName = "money")]
    public decimal? TongTien { get; set; }
}
