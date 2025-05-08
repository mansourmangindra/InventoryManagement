using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.DbContexts.InventoryManagement.Models;

[Table("ErrorLog")]
public partial class ErrorLog
{
    [Key]
    public int ErrorId { get; set; }

    [StringLength(4000)]
    public string Message { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateCreated { get; set; }

    public string StackTrace { get; set; } = null!;

    [StringLength(500)]
    public string Path { get; set; } = null!;

    [StringLength(100)]
    public string StackTraceId { get; set; } = null!;

    [StringLength(100)]
    public string Source { get; set; } = null!;

    [StringLength(100)]
    public string UserIdentity { get; set; } = null!;

    [StringLength(50)]
    public string BuildVersion { get; set; } = null!;
}
