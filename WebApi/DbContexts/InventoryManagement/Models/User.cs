using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.DbContexts.InventoryManagement.Models;

[Table("User")]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(100)]
    public string EmailAddress { get; set; } = null!;

    [StringLength(500)]
    public string Password { get; set; } = null!;

    [StringLength(100)]
    public string CreatedBy { get; set; } = null!;

    [StringLength(100)]
    public string UpdatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Active { get; set; }
}
