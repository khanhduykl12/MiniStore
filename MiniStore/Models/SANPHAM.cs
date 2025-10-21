﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("SANPHAM")]
public partial class SANPHAM
{
    [StringLength(10)]
    [Unicode(false)]
    public string MALOAI { get; set; } = null!;

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MASP { get; set; } = null!;

    [StringLength(100)]
    public string? TENSP { get; set; }

    public DateOnly? NSX { get; set; }

    [StringLength(10)]
    public string? DVT { get; set; }

    [Column(TypeName = "money")]
    public decimal? GIABAN { get; set; }

    public int? SOLUONG { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string MANCC { get; set; } = null!;

    [StringLength(40)]
    public string? GHICHU { get; set; }

    [StringLength(64)]
    [Unicode(false)]
    public string? BARCODE { get; set; }

    [StringLength(255)]
    public string? HINH { get; set; }

    [InverseProperty("MASPNavigation")]
    public virtual ICollection<CHITIETHDBAN> CHITIETHDBANs { get; set; } = new List<CHITIETHDBAN>();

    [InverseProperty("MASPNavigation")]
    public virtual ICollection<CHITIETHDNHAP> CHITIETHDNHAPs { get; set; } = new List<CHITIETHDNHAP>();

    [ForeignKey("MALOAI")]
    [InverseProperty("SANPHAMs")]
    public virtual LOAISANPHAM MALOAINavigation { get; set; } = null!;

    [ForeignKey("MANCC")]
    [InverseProperty("SANPHAMs")]
    public virtual NHACUNGCAP MANCCNavigation { get; set; } = null!;
}
