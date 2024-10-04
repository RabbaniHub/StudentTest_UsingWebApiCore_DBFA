﻿using System;
using System.Collections.Generic;

namespace StudentTestUsingWebApiCoreDBFA.Models;

public partial class Answer
{
    public int AnswerId { get; set; }

    public string AnswerText { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }

    public virtual Question Question { get; set; } = null!;
}
