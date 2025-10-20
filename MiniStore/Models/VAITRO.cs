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
    public int MAROLE { get; set; }

    [StringLength(50)]
    public string? MOTA { get; set; }

    [InverseProperty("MAROLENavigation")]
    public virtual ICollection<TAIKHOAN> TAIKHOANs { get; set; } = new List<TAIKHOAN>();
}
