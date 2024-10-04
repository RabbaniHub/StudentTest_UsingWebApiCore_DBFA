using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentTestUsingWebApiCoreDBFA.Models;

namespace StudentTestUsingWebApiCoreDBFA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public StudentController(StudentDbContext context)
        {
            _context = context;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] User model)
        {
            if (_context.Users.Any(u => u.UserName == model.UserName || u.Email == model.Email))
            {
                return BadRequest("Username or Email already exists.");
            }

            _context.Users.Add(model);
            _context.SaveChanges();
            return Ok();
        }

        // POST: api/user/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and password are required.");
            }
            var user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var loginRecord = new UserLogin
            {
                UserId = user.UserId,
                LoginTime = DateTime.Now,
                IsLoggedIn = true
            };
            _context.UserLogins.Add(loginRecord);
            _context.SaveChangesAsync();

            return Ok();
        }

        // GET: api/user/start-test
        [HttpGet("startTest")]
        public IActionResult StartTest()
        {
            var questions = _context.Questions
                .Select(q => new
                {
                    q.QuestionId,
                    q.QuestionText,
                    q.Explanation,
                    Answers = q.Answers.Select(a => new
                    {
                        a.AnswerId,
                        a.AnswerText
                    }).ToList()
                })
                .OrderBy(q => Guid.NewGuid())
                .Take(5)
                .ToList();

            return Ok(questions);
        }

        // POST: api/user/submit-test
        [HttpPost("submit-test")]
        public IActionResult SubmitTest([FromBody] List<TestSubmission> answers)
        {
            int correctAnswers = 0;

            foreach (var submission in answers)
            {
                var correctAnswer = _context.Answers
                    .FirstOrDefault(a => a.QuestionId == submission.QuestionId && a.IsCorrect);

                if (correctAnswer != null && correctAnswer.AnswerId == submission.SelectedAnswerId)
                {
                    correctAnswers++;
                }
            }

            return Ok(new { Score = correctAnswers });
        }

        // GET: api/user/result/{score}
        [HttpGet("result/{score}")]
        public IActionResult Result(int score)
        {
            int totalQuestions = _context.Answers.Where(a => a.IsCorrect).Count();
            double percentageScore = ((double)score / totalQuestions) * 100;

            return Ok(new
            {
                Score = score,
                TotalQuestions = totalQuestions,
                PercentageScore = percentageScore
            });
        }
    }
}
