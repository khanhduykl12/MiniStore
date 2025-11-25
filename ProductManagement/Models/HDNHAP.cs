using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("HDNHAP")]
public partial class HDNHAP
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MAHDNHAP { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime NGAYLAP { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? USERNAME { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string MANCC { get; set; } = null!;

    [StringLength(225)]
    public string? GHICHU { get; set; }

    [InverseProperty("MAHDNHAPNavigation")]
    public virtual ICollection<CHITIETHDNHAP> CHITIETHDNHAPs { get; set; } = new List<CHITIETHDNHAP>();

    [InverseProperty("MAHD_NHAPNavigation")]
    public virtual ICollection<CONGNO> CONGNOs { get; set; } = new List<CONGNO>();

    [ForeignKey("MANCC")]
    [InverseProperty("HDNHAPs")]
    public virtual NHACUNGCAP MANCCNavigation { get; set; } = null!;

    [ForeignKey("USERNAME")]
    [InverseProperty("HDNHAPs")]
    public virtual TAIKHOAN? USERNAMENavigation { get; set; }
}
