using System;
using System.Collections.Generic;

namespace TaskMgmtApi.Context;

public partial class Task
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string? Label { get; set; }

    public string? Type { get; set; }

    public string? Name { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? Duration { get; set; }

    public double? Progress { get; set; }

    public bool? IsUnscheduled { get; set; }

    public int Status { get; set; }
}
