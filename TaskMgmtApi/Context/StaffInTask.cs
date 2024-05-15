using System;
using System.Collections.Generic;

namespace TaskMgmtApi.Context;

public partial class StaffInTask
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public int TaskId { get; set; }

    public int Status { get; set; }
}
