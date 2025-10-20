using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("NHANVIEN")]
[Index("USERNAME", Name = "UQ__NHANVIEN__B15BE12E589E87B1", IsUnique = true)]
public partial class NHANVIEN
{
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

    [StringLength(10)]
    public string? CHUCVU { get; set; }

    [Column(TypeName = "money")]
    public decimal? LUONG { get; set; }

    [Key]
    [StringLength(20)]
    [Unicode(false)]
    public string USERNAME { get; set; } = null!;

    [InverseProperty("USERNAMENavigation")]
    public virtual ICollection<HDBAN> HDBANs { get; set; } = new List<HDBAN>();

    [InverseProperty("USERNAMENavigation")]
    public virtual ICollection<HDNHAP> HDNHAPs { get; set; } = new List<HDNHAP>();

    [ForeignKey("USERNAME")]
    [InverseProperty("NHANVIEN")]
    public virtual TAIKHOAN USERNAMENavigation { get; set; } = null!;
}
