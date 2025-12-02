using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("NHACUNGCAP")]
public partial class NHACUNGCAP
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MANCC { get; set; } = null!;

    [StringLength(30)]
    public string? TENNCC { get; set; }

    [StringLength(40)]
    public string? DIACHI { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? SDT { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? EMAIL { get; set; }

    [InverseProperty("MANCCNavigation")]
    public virtual ICollection<CONGNO> CONGNOs { get; set; } = new List<CONGNO>();

    [InverseProperty("MANCCNavigation")]
    public virtual ICollection<HDNHAP> HDNHAPs { get; set; } = new List<HDNHAP>();

    [InverseProperty("MANCCNavigation")]
    public virtual ICollection<SANPHAM> SANPHAMs { get; set; } = new List<SANPHAM>();
}
