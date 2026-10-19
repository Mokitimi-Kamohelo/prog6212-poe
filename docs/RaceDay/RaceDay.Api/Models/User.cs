using System;
using System.Collections.Generic;

namespace RaceDay.Api.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public string? Phone { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual Role Role { get; set; } = null!;
}
