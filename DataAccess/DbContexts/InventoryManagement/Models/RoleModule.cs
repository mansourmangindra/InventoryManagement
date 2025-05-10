using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts.InventoryManagement.Models;

[Table("RoleModule")]
public partial class RoleModule
{
    [Key]
    public int RoleModuleId { get; set; }

    public short RoleId { get; set; }

    public short ModuleId { get; set; }

    [Required]
    [StringLength(100)]
    public string UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime UpdatedDate { get; set; }

    public bool Active { get; set; }

    [ForeignKey("ModuleId")]
    [InverseProperty("RoleModules")]
    public virtual Module Module { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("RoleModules")]
    public virtual Role Role { get; set; }
}
