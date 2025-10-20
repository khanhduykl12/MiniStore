using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("HDBAN")]
public partial class HDBAN
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MAHD { get; set; } = null!;

    public DateOnly NGAYLAP { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? USERNAME { get; set; }

    [StringLength(225)]
    public string? GHICHU { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MAKH { get; set; }

    [InverseProperty("MAHDNavigation")]
    public virtual ICollection<CHITIETHDBAN> CHITIETHDBANs { get; set; } = new List<CHITIETHDBAN>();

    [ForeignKey("MAKH")]
    [InverseProperty("HDBANs")]
    public virtual KHACHHANG? MAKHNavigation { get; set; }

    [ForeignKey("USERNAME")]
    [InverseProperty("HDBANs")]
    public virtual NHANVIEN? USERNAMENavigation { get; set; }
}
