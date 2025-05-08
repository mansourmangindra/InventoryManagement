using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.DbContexts.InventoryManagement.Models;

[Table("AuditTrail")]
public partial class AuditTrail
{
    [Key]
    public long AuditTrailId { get; set; }

    [StringLength(200)]
    public string TransactionBy { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    [InverseProperty("AuditTrail")]
    public virtual ICollection<AuditTrailDetail> AuditTrailDetails { get; set; } = new List<AuditTrailDetail>();
}
