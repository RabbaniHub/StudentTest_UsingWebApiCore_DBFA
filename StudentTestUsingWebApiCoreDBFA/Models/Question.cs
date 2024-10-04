using System;
using System.Collections.Generic;

namespace StudentTestUsingWebApiCoreDBFA.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public string QuestionText { get; set; } = null!;

    public string Explanation { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<TestSubmission> TestSubmissions { get; set; } = new List<TestSubmission>();
}
