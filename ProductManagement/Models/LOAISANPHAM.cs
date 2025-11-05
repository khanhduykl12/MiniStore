using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Models;

[Table("LOAISANPHAM")]
public partial class LOAISANPHAM
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MALOAI { get; set; } = null!;

    [StringLength(100)]
    public string? TENLOAI { get; set; }

    public int? HSD_NGAY { get; set; }

    [InverseProperty("MALOAINavigation")]
    public virtual ICollection<SANPHAM> SANPHAMs { get; set; } = new List<SANPHAM>();
}
