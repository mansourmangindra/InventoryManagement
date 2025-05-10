using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts.InventoryManagement.Models;

[Table("Module")]
public partial class Module
{
    [Key]
    public short ModuleId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Active { get; set; }

    [InverseProperty("Module")]
    public virtual ICollection<RoleModule> RoleModules { get; set; } = new List<RoleModule>();
}
