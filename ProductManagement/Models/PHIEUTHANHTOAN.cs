using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("PHIEUTHANHTOAN")]
public partial class PHIEUTHANHTOAN
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MAPTT { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string MACONGNO { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal SOTIENTRA { get; set; }

    public DateOnly NGAYTRA { get; set; }

    [StringLength(100)]
    public string? GHICHU { get; set; }

    [ForeignKey("MACONGNO")]
    [InverseProperty("PHIEUTHANHTOANs")]
    public virtual CONGNO MACONGNONavigation { get; set; } = null!;
}
