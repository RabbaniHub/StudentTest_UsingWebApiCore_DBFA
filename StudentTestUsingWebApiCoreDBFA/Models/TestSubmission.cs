using System;
using System.Collections.Generic;

namespace StudentTestUsingWebApiCoreDBFA.Models;

public partial class TestSubmission
{
    public int SelectedAnswerId { get; set; }

    public int QuestionId { get; set; }

    public virtual Question Question { get; set; } = null!;
}
