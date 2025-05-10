using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts.InventoryManagement.Models;

[Keyless]
[Table("UserProfile")]
public partial class UserProfile
{
    public int UserProfileId { get; set; }

    public int UserId { get; set; }

    [Required]
    [StringLength(255)]
    public string ProfileImageUrl { get; set; }

    [Required]
    [StringLength(500)]
    public string Bio { get; set; }

    [Required]
    [StringLength(10)]
    public string Gender { get; set; }

    [Required]
    [StringLength(100)]
    public string CreatedBy { get; set; }

    [Required]
    [StringLength(100)]
    public string UpdatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Active { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }
}
