using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("TAIKHOAN")]
[Index("EMAIL", Name = "UQ__TAIKHOAN__161CF7248DF6BE6D", IsUnique = true)]
public partial class TAIKHOAN
{
    [Key]
    [StringLength(20)]
    [Unicode(false)]
    public string USERNAME { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string PASSWORD { get; set; } = null!;

    [StringLength(10)]
    public string MAROLE { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string EMAIL { get; set; } = null!;

    [StringLength(50)]
    public string? TRANGTHAI { get; set; }

    public DateOnly? NGAYKHOA { get; set; }

    public DateOnly? NGAYMOKHOA { get; set; }

    [ForeignKey("MAROLE")]
    [InverseProperty("TAIKHOANs")]
    public virtual VAITRO MAROLENavigation { get; set; } = null!;

    [InverseProperty("USERNAMENavigation")]
    public virtual NGUOIDUNG? NGUOIDUNG { get; set; }

    [InverseProperty("USERNAMENavigation")]
    public virtual ICollection<OTP_LOG> OTP_LOGs { get; set; } = new List<OTP_LOG>();
}
