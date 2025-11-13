using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[PrimaryKey("MAHD", "MASP")]
[Table("CHITIETHDBAN")]
public partial class CHITIETHDBAN
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MAHD { get; set; } = null!;

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MASP { get; set; } = null!;

    public int? SOLUONG { get; set; }

    [Column(TypeName = "money")]
    public decimal? DONGIA { get; set; }

    [Column(TypeName = "money")]
    public decimal? THANHTIEN { get; set; }

    [ForeignKey("MAHD")]
    [InverseProperty("CHITIETHDBANs")]
    public virtual HDBAN MAHDNavigation { get; set; } = null!;

    [ForeignKey("MASP")]
    [InverseProperty("CHITIETHDBANs")]
    public virtual SANPHAM MASPNavigation { get; set; } = null!;
}
