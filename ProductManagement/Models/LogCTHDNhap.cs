using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("LogCTHDNhap")]
public partial class LogCTHDNhap
{
    [Key]
    public int LogID { get; set; }

    [StringLength(50)]
    public string? MAHDNHAP { get; set; }

    [StringLength(50)]
    public string? MASP { get; set; }

    public int? SOLUONGTN { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DONGIANHAP { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? THANHTIENN { get; set; }

    public string? GHICHU { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LoggedAt { get; set; }
}
