using System;
using System.Collections.Generic;

namespace RaceDay.Api.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public int EventId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string CategoryType { get; set; } = null!;

    public string? CategoryValue { get; set; }

    public virtual ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();

    public virtual Event Event { get; set; } = null!;
}
