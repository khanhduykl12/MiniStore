using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("CHAMCONG")]
public partial class CHAMCONG
{
    [Key]
    public int MACHAM { get; set; }

    public int MANV { get; set; }

    public DateOnly NGAYLAM { get; set; }

    public TimeOnly? GIOVAO { get; set; }

    public TimeOnly? GIORA { get; set; }

    public double? SOGIO { get; set; }

    [StringLength(20)]
    public string? TRANGTHAI { get; set; }

    public int? LICHLAM_ID { get; set; }

    [ForeignKey("LICHLAM_ID")]
    [InverseProperty("CHAMCONGs")]
    public virtual LICHLAM? LICHLAM { get; set; }

    [ForeignKey("MANV")]
    [InverseProperty("CHAMCONGs")]
    public virtual NGUOIDUNG MANVNavigation { get; set; } = null!;
}
