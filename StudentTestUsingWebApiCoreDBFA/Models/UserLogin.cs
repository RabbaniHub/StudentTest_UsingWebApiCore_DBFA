using System;
using System.Collections.Generic;

namespace StudentTestUsingWebApiCoreDBFA.Models;

public partial class UserLogin
{
    public int UserLoginId { get; set; }

    public int UserId { get; set; }

    public DateTime LoginTime { get; set; }

    public bool IsLoggedIn { get; set; }

    public virtual User User { get; set; } = null!;
}
