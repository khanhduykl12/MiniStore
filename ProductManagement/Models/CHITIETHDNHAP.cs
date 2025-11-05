using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Models;

[PrimaryKey("MAHDNHAP", "MASP")]
[Table("CHITIETHDNHAP")]
public partial class CHITIETHDNHAP
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MAHDNHAP { get; set; } = null!;

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MASP { get; set; } = null!;

    public int SOLUONGTN { get; set; }

    public int DONGIANHAP { get; set; }

    public int? THANHTIENN { get; set; }

    [StringLength(225)]
    public string? GHICHU { get; set; }

    [ForeignKey("MAHDNHAP")]
    [InverseProperty("CHITIETHDNHAPs")]
    public virtual HDNHAP MAHDNHAPNavigation { get; set; } = null!;

    [ForeignKey("MASP")]
    [InverseProperty("CHITIETHDNHAPs")]
    public virtual SANPHAM MASPNavigation { get; set; } = null!;
}
