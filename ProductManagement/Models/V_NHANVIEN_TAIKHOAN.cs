using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Models;

[Keyless]
public partial class V_NHANVIEN_TAIKHOAN
{
    [StringLength(20)]
    [Unicode(false)]
    public string USERNAME { get; set; } = null!;

    [StringLength(30)]
    public string HOTEN { get; set; } = null!;

    [StringLength(3)]
    public string? GioiTinh { get; set; }

    public DateOnly? NGAYSINH { get; set; }

    [StringLength(40)]
    public string? DIACHI { get; set; }

    [StringLength(10)]
    public string? CHUCVU { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? SDT { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string EMAIL { get; set; } = null!;
}
