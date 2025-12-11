using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("LICHLAM")]
public partial class LICHLAM
{
    [Key]
    public int MALICH { get; set; }

    public int NGUOIDUNG_ID { get; set; }

    public DateOnly NGAYLAM { get; set; }

    [StringLength(20)]
    public string? CALAM { get; set; }

    public TimeOnly? GIOVAO { get; set; }

    public TimeOnly? GIORA { get; set; }

    [StringLength(100)]
    public string? GHICHU { get; set; }

    [InverseProperty("LICHLAM")]
    public virtual ICollection<CHAMCONG> CHAMCONGs { get; set; } = new List<CHAMCONG>();

    [ForeignKey("NGUOIDUNG_ID")]
    [InverseProperty("LICHLAMs")]
    public virtual NGUOIDUNG NGUOIDUNG { get; set; } = null!;
}
