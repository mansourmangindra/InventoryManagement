using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts.InventoryManagement.Models;

[Table("Role")]
public partial class Role
{
    [Key]
    public short RoleId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string CreatedBy { get; set; }

    [Required]
    [StringLength(100)]
    public string UpdatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Active { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<RoleModule> RoleModules { get; set; } = new List<RoleModule>();

    [InverseProperty("Role")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
