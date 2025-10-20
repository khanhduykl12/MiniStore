using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("KHACHHANG")]
[Index("USERNAME", Name = "UQ__KHACHHAN__B15BE12E85889E83", IsUnique = true)]
public partial class KHACHHANG
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MAKH { get; set; } = null!;

    [StringLength(6)]
    [Unicode(false)]
    public string LOAIKH { get; set; } = null!;

    [StringLength(20)]
    public string? HOTEN { get; set; }

    public DateOnly? NGAYSINH { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? SDT { get; set; }

    [StringLength(3)]
    public string? GioiTinh { get; set; }

    [StringLength(30)]
    public string? DIACHI { get; set; }

    public int? DIEMTICHLUY { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? USERNAME { get; set; }

    [InverseProperty("MAKHNavigation")]
    public virtual ICollection<HDBAN> HDBANs { get; set; } = new List<HDBAN>();

    [ForeignKey("USERNAME")]
    [InverseProperty("KHACHHANG")]
    public virtual TAIKHOAN? USERNAMENavigation { get; set; }
}
