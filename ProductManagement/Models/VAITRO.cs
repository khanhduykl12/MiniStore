using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("VAITRO")]
public partial class VAITRO
{
    [Key]
    [StringLength(10)]
    public string MAROLE { get; set; } = null!;

    [StringLength(50)]
    public string? MOTA { get; set; }

    [InverseProperty("MAROLENavigation")]
    public virtual ICollection<NGUOIDUNG> NGUOIDUNGs { get; set; } = new List<NGUOIDUNG>();

    [InverseProperty("MAROLENavigation")]
    public virtual ICollection<TAIKHOAN> TAIKHOANs { get; set; } = new List<TAIKHOAN>();
}
