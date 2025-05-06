using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts.InventoryManagement.Models;

[Table("AuditTrailDetail")]
public partial class AuditTrailDetail
{
    [Key]
    public long AuditTrailDetailId { get; set; }

    public long AuditTrailId { get; set; }

    public string EntityId { get; set; } = null!;

    [StringLength(50)]
    public string TableName { get; set; } = null!;

    [StringLength(50)]
    public string EntityField { get; set; } = null!;

    public string OldValue { get; set; } = null!;

    public string NewValue { get; set; } = null!;

    [StringLength(100)]
    public string Action { get; set; } = null!;

    [ForeignKey("AuditTrailId")]
    [InverseProperty("AuditTrailDetails")]
    public virtual AuditTrail AuditTrail { get; set; } = null!;
}
