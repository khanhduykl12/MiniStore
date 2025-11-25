using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("HANGTRUNGBAY")]
public partial class HANGTRUNGBAY
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MASP { get; set; } = null!;

    public int SOLUONG_TRENKE { get; set; }

    [ForeignKey("MASP")]
    [InverseProperty("HANGTRUNGBAY")]
    public virtual SANPHAM MASPNavigation { get; set; } = null!;
}
