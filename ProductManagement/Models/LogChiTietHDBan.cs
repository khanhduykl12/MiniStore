using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("LogChiTietHDBan")]
public partial class LogChiTietHDBan
{
    [Key]
    public int LogID { get; set; }

    [StringLength(20)]
    public string? MaHD { get; set; }

    [StringLength(20)]
    public string? MaSP { get; set; }

    public int? SoLuong { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DonGia { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ThanhTien { get; set; }

    [StringLength(10)]
    public string? ActionType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LogDate { get; set; }
}
