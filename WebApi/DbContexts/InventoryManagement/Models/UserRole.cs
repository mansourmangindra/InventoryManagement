using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.DbContexts.InventoryManagement.Models;

[Table("UserRole")]
public partial class UserRole
{
    [Key]
    public int UserRoleId { get; set; }

    public int UserId { get; set; }

    public short RoleId { get; set; }

    [StringLength(100)]
    public string CreatedBy { get; set; } = null!;

    [Column("UpdatedBY")]
    [StringLength(100)]
    public string UpdatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Active { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("UserRoles")]
    public virtual Role Role { get; set; } = null!;
}
