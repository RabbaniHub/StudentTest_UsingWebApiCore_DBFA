// wwwroot/js/user.js
async function registerUser() {
    const user = {
        userName: document.getElementById("username").value,
        email: document.getElementById("email").value,
        password: document.getElementById("password").value
    };

    const response = await fetch('/api/Student/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    });

    if (response.ok) {
        alert('Registration successful');
        window.location.href = '/login.html';
    } else {
        alert('Registration failed: ' + await response.text());
    }
}

async function loginUser() {
    const loginRequest = {
        userName: document.getElementById("uname").value,
        password: document.getElementById("pass").value
    };
    const response = await fetch('/api/Student/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginRequest)
    });

    if (response.ok) {
        window.location.href = '/test.html';
    } else {
        alert('Login failed: ' + await response.text());
    }
}

async function startTest() {
    try {
        const response = await fetch('/api/Student/starTtest');
        if (!response.ok) {
            throw new Error('Failed to fetch questions: ' + response.statusText);
        }

        const questions = await response.json();
        if (questions.length ===0) {
            document.getElementById('questions').innerHTML = '<p>No questions available.</p>';
            return;
        }

        let questionHtml = '';
        // Check if questions are coming in a valid array format
        if (Array.isArray(questions) && questions.length > 0) {
            debugger;
            questions.forEach((q, index) => {
                questionHtml += `<div class="question">
                <p>${index + 1}. ${q.QuestionText}</p>
                ${q.Answers.map(a => `
                    <label>
                        <input type="radio" name="q${q.QuestionId}" value="${a.AnswerId}">
                        ${a.AnswerText}
                    </label><br>`).join('')}
            </div>`;
            });
        } else {
            questionHtml = `<p>No questions available.</p>`;
        }

        document.getElementById('questions').innerHTML = questionHtml;
    } catch (error) {
        console.error('Error loading test questions:', error);
        document.getElementById('questions').innerHTML = '<p>Error loading test questions.</p>';
    }
}


async function submitTest() {
    const answers = [];

    document.querySelectorAll('.question').forEach(questionDiv => {
        const questionId = questionDiv.querySelector('input[type="radio"]').name.slice(1);
        const selectedAnswerId = questionDiv.querySelector('input[type="radio"]:checked')?.value;

        if (selectedAnswerId) {
            answers.push({ QuestionId: questionId, SelectedAnswerId: selectedAnswerId });
        }
    });

    const response = await fetch('/api/Student/submit-test', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(answers)
    });

    const result = await response.json();
    window.location.href = `/pages/result.html?score=${result.Score}`;
}

async function showResult() {
    const params = new URLSearchParams(window.location.search);
    const score = params.get('score');

    const response = await fetch(`/api/user/result/${score}`);
    const result = await response.json();

    document.getElementById('result').innerHTML = `
        <p>Your score: ${result.Score}</p>
        <p>Total questions: ${result.TotalQuestions}</p>
        <p>Percentage: ${result.PercentageScore.toFixed(2)}%</p>
    `;
}
