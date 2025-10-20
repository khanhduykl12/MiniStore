using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Keyless]
public partial class V_SANPHAM_NHACUNGCAP
{
    [StringLength(10)]
    [Unicode(false)]
    public string MASP { get; set; } = null!;

    [StringLength(100)]
    public string? TENSP { get; set; }

    public int? SOLUONG { get; set; }

    [StringLength(30)]
    public string? TENNCC { get; set; }
}
