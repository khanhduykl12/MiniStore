using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Keyless]
public partial class V_TONKHO
{
    [StringLength(10)]
    [Unicode(false)]
    public string MASP { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string MALOAI { get; set; } = null!;

    [StringLength(100)]
    public string? TENSP { get; set; }

    public DateOnly? NSX { get; set; }

    public DateOnly? HSD { get; set; }

    [StringLength(10)]
    public string? DVT { get; set; }

    [Column(TypeName = "money")]
    public decimal? GIABAN { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string MANCC { get; set; } = null!;

    public int SOLUONGNHAP { get; set; }

    public int SOLUONGBAN { get; set; }

    public int? SOLUONGTON { get; set; }
}
