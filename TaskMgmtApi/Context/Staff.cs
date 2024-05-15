using System;
using System.Collections.Generic;

namespace TaskMgmtApi.Context;

public partial class Staff
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? NameNonUnicode { get; set; }

    public string? ShortName { get; set; }

    public int Status { get; set; }
}
