using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("DONHANG_LOG")]
public partial class DONHANG_LOG
{
    [Key]
    public int ID { get; set; }

    [StringLength(64)]
    public string? MAKH { get; set; }

    [StringLength(128)]
    public string? TENKH { get; set; }

    [StringLength(128)]
    public string? EMAIL { get; set; }

    [StringLength(32)]
    public string? DIENTHOAI { get; set; }

    [StringLength(256)]
    public string? DIACHI { get; set; }

    [StringLength(32)]
    public string? TINHTHANH { get; set; }

    [StringLength(32)]
    public string? QUANHUYEN { get; set; }

    [StringLength(32)]
    public string? PHUONGXA { get; set; }

    [StringLength(32)]
    public string? PHUONGTHUCTHANHTOAN { get; set; }

    [Column(TypeName = "money")]
    public decimal TONGTIEN { get; set; }

    public DateTime CREATED_AT { get; set; }

    [StringLength(32)]
    public string TRANGTHAI { get; set; } = null!;

    public string? CHITIET_JSON { get; set; }
}
