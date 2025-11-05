using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Models;

[Table("CONGNO")]
public partial class CONGNO
{
    [Key]
    [StringLength(20)]
    [Unicode(false)]
    public string MACONGNO { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string MAHD_NHAP { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string MANCC { get; set; } = null!;

    public DateOnly NGAYPHATSINH { get; set; }

    [Column(TypeName = "money")]
    public decimal SOTIENPHAITRA { get; set; }

    [Column(TypeName = "money")]
    public decimal DATHANHTOAN { get; set; }

    [Column(TypeName = "money")]
    public decimal? CONLAI { get; set; }

    public DateOnly? HANTRA { get; set; }

    [StringLength(50)]
    public string? TRANGTHAI { get; set; }

    [StringLength(500)]
    public string? GHICHU { get; set; }

    [ForeignKey("MAHD_NHAP")]
    [InverseProperty("CONGNOs")]
    public virtual HDNHAP MAHD_NHAPNavigation { get; set; } = null!;

    [ForeignKey("MANCC")]
    [InverseProperty("CONGNOs")]
    public virtual NHACUNGCAP MANCCNavigation { get; set; } = null!;

    [InverseProperty("MACONGNONavigation")]
    public virtual ICollection<PHIEUTHANHTOAN> PHIEUTHANHTOANs { get; set; } = new List<PHIEUTHANHTOAN>();
}
