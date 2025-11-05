using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Models;

[Table("HDBAN")]
public partial class HDBAN
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MAHD { get; set; } = null!;

    public DateOnly NGAYLAP { get; set; }

    public int NGUOILAP_ID { get; set; }

    public int NGUOIMUA_ID { get; set; }

    [StringLength(200)]
    public string? GHICHU { get; set; }

    [InverseProperty("MAHDNavigation")]
    public virtual ICollection<CHITIETHDBAN> CHITIETHDBANs { get; set; } = new List<CHITIETHDBAN>();

    [ForeignKey("NGUOILAP_ID")]
    [InverseProperty("HDBANNGUOILAPs")]
    public virtual NGUOIDUNG NGUOILAP { get; set; } = null!;

    [ForeignKey("NGUOIMUA_ID")]
    [InverseProperty("HDBANNGUOIMUAs")]
    public virtual NGUOIDUNG NGUOIMUA { get; set; } = null!;
}
