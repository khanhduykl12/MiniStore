using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Keyless]
public partial class VIEW_ThongKeDoanhThu
{
    public DateOnly? Ngay { get; set; }

    [Column(TypeName = "money")]
    public decimal DoanhThu { get; set; }

    public int ChiPhi { get; set; }

    [Column(TypeName = "money")]
    public decimal? LoiNhuan { get; set; }
}
