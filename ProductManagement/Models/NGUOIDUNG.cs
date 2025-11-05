using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Models;

[Table("NGUOIDUNG")]
[Index("USERNAME", Name = "UQ__NGUOIDUN__B15BE12E612F915C", IsUnique = true)]
public partial class NGUOIDUNG
{
    [Key]
    public int ID { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string USERNAME { get; set; } = null!;

    [StringLength(30)]
    public string HOTEN { get; set; } = null!;

    public DateOnly? NGAYSINH { get; set; }

    [StringLength(3)]
    public string? GioiTinh { get; set; }

    [StringLength(40)]
    public string? DIACHI { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? SDT { get; set; }

    [StringLength(30)]
    public string? CHUCVU { get; set; }

    [Column(TypeName = "money")]
    public decimal? LUONG { get; set; }

    public int? DIEMTICHLUY { get; set; }

    [StringLength(10)]
    public string? MAROLE { get; set; }

    [InverseProperty("NGUOILAP")]
    public virtual ICollection<HDBAN> HDBANNGUOILAPs { get; set; } = new List<HDBAN>();

    [InverseProperty("NGUOIMUA")]
    public virtual ICollection<HDBAN> HDBANNGUOIMUAs { get; set; } = new List<HDBAN>();

    [InverseProperty("USERNAMENavigation")]
    public virtual ICollection<HDNHAP> HDNHAPs { get; set; } = new List<HDNHAP>();

    [ForeignKey("USERNAME")]
    [InverseProperty("NGUOIDUNG")]
    public virtual TAIKHOAN USERNAMENavigation { get; set; } = null!;
}
