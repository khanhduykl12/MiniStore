using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

[Table("OTP_LOG")]
public partial class OTP_LOG
{
    [Key]
    public int ID { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string USERNAME { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string EMAIL { get; set; } = null!;

    [StringLength(6)]
    [Unicode(false)]
    public string OTP_CODE { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CREATE_AT { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EXPIRY_AT { get; set; }

    [ForeignKey("USERNAME")]
    [InverseProperty("OTP_LOGs")]
    public virtual TAIKHOAN USERNAMENavigation { get; set; } = null!;
}
