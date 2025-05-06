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

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string CreatedBy { get; set; } = null!;

    [StringLength(100)]
    public string UpdatedBy { get; set; } = null!;

    [StringLength(100)]
    public string CreatedDate { get; set; } = null!;

    [StringLength(100)]
    public string UpdatedDate { get; set; } = null!;

    public bool Active { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
