using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("TAIKHOAN")]
[Index("EMAIL", Name = "UQ__TAIKHOAN__161CF724FCA80804", IsUnique = true)]
public partial class TAIKHOAN
{
    [Key]
    [StringLength(20)]
    [Unicode(false)]
    public string USERNAME { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string PASSWORD { get; set; } = null!;

    public int MAROLE { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string EMAIL { get; set; } = null!;

    [InverseProperty("USERNAMENavigation")]
    public virtual KHACHHANG? KHACHHANG { get; set; }

    [ForeignKey("MAROLE")]
    [InverseProperty("TAIKHOANs")]
    public virtual VAITRO MAROLENavigation { get; set; } = null!;

    [InverseProperty("USERNAMENavigation")]
    public virtual NHANVIEN? NHANVIEN { get; set; }
}
