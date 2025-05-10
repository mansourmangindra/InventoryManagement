using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts.InventoryManagement.Models;

[Table("AuditTrail")]
public partial class AuditTrail
{
    [Key]
    public long AuditTrailId { get; set; }

    [Required]
    [StringLength(200)]
    public string TransactionBy { get; set; }

    public DateTime TransactionDate { get; set; }

    [InverseProperty("AuditTrail")]
    public virtual ICollection<AuditTrailDetail> AuditTrailDetails { get; set; } = new List<AuditTrailDetail>();
}
