using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Keyless]
public partial class V_SANPHAM_HSD
{
    [StringLength(10)]
    [Unicode(false)]
    public string MASP { get; set; } = null!;

    [StringLength(100)]
    public string? TENSP { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string MALOAI { get; set; } = null!;

    [StringLength(20)]
    public string? TENLOAI { get; set; }

    public DateOnly? NSX { get; set; }

    public int? HSD_NGAY { get; set; }

    public DateOnly? HSD { get; set; }

    public int? NgayConLai { get; set; }

    [StringLength(21)]
    public string TinhTrang { get; set; } = null!;
}
