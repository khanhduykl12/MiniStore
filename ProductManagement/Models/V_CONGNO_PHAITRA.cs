using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Keyless]
public partial class V_CONGNO_PHAITRA
{
    [StringLength(20)]
    [Unicode(false)]
    public string MACONGNO { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string MAHD { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string MANCC { get; set; } = null!;

    [StringLength(30)]
    public string? TENNHACUNGCAP { get; set; }

    public DateOnly NGAYPHATSINH { get; set; }

    [Column(TypeName = "money")]
    public decimal SOTIENPHAITRA { get; set; }

    [Column(TypeName = "money")]
    public decimal DATHANHTOAN { get; set; }

    [Column(TypeName = "money")]
    public decimal? CONLAI { get; set; }

    public DateOnly? HANTRA { get; set; }

    [StringLength(50)]
    public string? TRANGTHAI { get; set; }

    [StringLength(500)]
    public string? GHICHU { get; set; }
}
