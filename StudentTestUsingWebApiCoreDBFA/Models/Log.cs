using System;
using System.Collections.Generic;

namespace StudentTestUsingWebApiCoreDBFA.Models;

public partial class Log
{
    public int LogId { get; set; }

    public string Action { get; set; } = null!;

    public DateTime TimeStamp { get; set; }

    public string Details { get; set; } = null!;
}
