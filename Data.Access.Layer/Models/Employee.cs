using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Access.Layer.Models;

public partial class Employee
{
    [Key]
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string PhotoUrl { get; set; } = null!;

    public int Empcode { get; set; }

    public virtual ICollection<Gaurd> Gaurds { get; } = new List<Gaurd>();
}
