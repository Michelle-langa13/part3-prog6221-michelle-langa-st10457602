using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;

namespace SECURITY_AI   // ✅ FIXED: removed space
{
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer speech = new SpeechSynthesizer();

        // =========================
        // NAME MEMORY
        // =========================
        private string userName = "";

        // =========================
        // TASKS
        // =========================
        class TaskItem
        {
            public string Title { get; set; }
            public DateTime Created { get; set; }
        }

        private List<TaskItem> tasks = new List<TaskItem>();

        // =========================
        // REMINDERS
        // =========================
        class Reminder
        {
            public string Message { get; set; }
            public DateTime Time { get; set; }
        }

        private List<Reminder> reminders = new List<Reminder>();

        // =========================
        // QUIZ SYSTEM
        // =========================
        private class QuizQuestion
        {
            public string Question { get; set; }
            public string Type { get; set; }
            public bool? TrueFalseAnswer { get; set; }
            public List<string> Options { get; set; }
            public string CorrectOption { get; set; }
        }

        private List<QuizQuestion> quiz = new List<QuizQuestion>
{
    new QuizQuestion
    {
        Question = "VPN encrypts your internet connection",
        Type = "tf",
        TrueFalseAnswer = true
    },

    new QuizQuestion
    {
        Question = "Phishing is used to trick users into giving personal information",
        Type = "tf",
        TrueFalseAnswer = true
    },

    new QuizQuestion
    {
        Question = "What does VPN stand for?",
        Type = "mcq",
        Options = new List<string>
        {
            "Virtual Private Network",
            "Very Personal Network",
            "Visual Private Node",
            "Virtual Public Network"
        },
        CorrectOption = "Virtual Private Network"
    },

    new QuizQuestion
    {
        Question = "What is malware?",
        Type = "mcq",
        Options = new List<string>
        {
            "Helpful software",
            "Harmful software",
            "Gaming software",
            "Security update"
        },
        CorrectOption = "Harmful software"
    }
};

        private int quizIndex = 0;
        private int score = 0;
        private bool quizMode = false;

        private Random random = new Random();

        // =========================
        // 🆕 LOG SYSTEM (ADDED)
        // =========================
        private List<string> logs = new List<string>();

        // =========================
        // 🆕 JOKE SYSTEM (ADDED)
        // =========================
        private List<string> jokes = new List<string>
        {
            "Why did the computer get cold? It left its Windows open 😄",
            "Why do programmers prefer dark mode? Because light attracts bugs 😆",
            "Why was the computer tired? It had too many tabs open 😂",
            "What is a hacker’s favorite place? The internet 😎"
        };

        // =========================
        // INIT
        // =========================
        public MainWindow()
        {
            InitializeComponent();

            speech.Volume = 100;
            speech.Rate = 0;

            ChatDisplay.AppendText(
@"=====================================
🛡 CYBERSHIELD AI SYSTEM ONLINE
=====================================
Type 'help' for commands
=====================================
");

            BotMessage("CyberShield Online 🛡");
        }

        // =========================
        // INPUT
        // =========================
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessMessage();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ProcessMessage();
        }

        // =========================
        // PROCESS
        // =========================
        private void ProcessMessage()
        {
            string input = UserInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                BotMessage("⚠ Enter a message.");
                return;
            }

            logs.Add(input);

            UserMessage(input);
            string response = GenerateResponse(input);
            BotMessage(response);

            UserInput.Clear();
        }

        private void UserMessage(string msg)
        {
            ChatDisplay.AppendText("\nYOU:\n" + msg + "\n");
        }

        private async void BotMessage(string msg)
        {
            ChatDisplay.AppendText("\nCyberShield:\n");

            foreach (char c in msg)
            {
                ChatDisplay.AppendText(c.ToString());
                await Task.Delay(8);
            }

            ChatDisplay.AppendText("\n");

            Speak(msg);
        }

        private void Speak(string text)
        {
            try
            {
                speech.SpeakAsyncCancelAll();
                speech.Speak(text);
            }
            catch { }
        }

        private string Help()
        {
            return @"🆘 CYBERSHIELD COMMANDS

👤 PERSONAL
- my name is ...
- hello

📋 TASKS
- add task ...
- show tasks
- edit task [index] [new text]
- delete task [index]

⏰ REMINDERS
- remind me in X seconds to ...

🧠 QUIZ GAME
- quiz

😂 FUN
- joke
- show logs

🕒 TIME & DATE
- time
- date

🔐 CYBERSECURITY INFO
- vpn
- virus
- malware
- phishing
- firewall
- password

💡 TIP
Type naturally like:
- add task study for exams
- remind me in 10 seconds to drink water
- my name is Alex";
        }

        private string GetJoke()
        {
            return jokes[random.Next(jokes.Count)];
        }

        private string SecurityInfo(string text)
        {
            if (text.Contains("vpn"))
                return "A VPN encrypts your internet connection to keep your online activity private." +
                    "\r\nIt helps protect your data, especially when using public WiFi.";

            if (text.Contains("virus"))
                return "A computer virus is a type of malware that spreads and damages files or systems." +
                    "\r\nIt can infect your device through downloads or unsafe links.";

            if (text.Contains("malware"))
                return "Malware is harmful software designed to damage or steal data from a device." +
                    "\r\nIt can slow down your system or give hackers access to your information.";

            if (text.Contains("phishing"))
                return "Phishing is when attackers trick you into giving personal information through fake messages or websites." +
                    "\r\nAlways check links carefully before entering any details.";

            if (text.Contains("firewall"))
                return "A firewall is a security system that blocks unauthorized access to your device or network." +
                    "\r\nIt helps protect your computer from hackers and threats.";

            if (text.Contains("password"))
                return "A password is a secret code used to protect your accounts and personal data." +
                    "\r\nStrong passwords help stop hackers from breaking into your accounts.";

            return null;
        }

        private string GenerateResponse(string input)
        {
            string text = input.ToLower();

            if (text.Contains("help"))
                return Help();

            if (text.Contains("my name is"))
            {
                userName = input.Replace("my name is", "").Trim();
                return $"Nice to meet you, {userName} 👋";
            }

            if (text.Contains("quiz"))
            {
                quizMode = true;
                quizIndex = 0;
                score = 0;

                return ShowQuestion();
            }

            if (quizMode)
                return HandleQuiz(text);

            string security = SecurityInfo(text);
            if (security != null)
                return security;

            if (text.Contains("joke"))
                return GetJoke();

            if (text.Contains("show logs"))
            {
                if (logs.Count == 0)
                    return "No logs yet.";

                string output = "📜 LOG HISTORY:\n\n";

                for (int i = 0; i < logs.Count; i++)
                {
                    output += $"{i + 1}. {logs[i]}\n";
                }

                return output;
            }

            if (text.Contains("add task"))
            {
                string task = input.Replace("add task", "").Trim();

                if (!string.IsNullOrEmpty(task))
                {
                    tasks.Add(new TaskItem
                    {
                        Title = task,
                        Created = DateTime.Now
                    });

                    return "Task added ✔";
                }

                return "Please type a task after 'add task'";
            }

            if (text.Contains("show tasks"))
            {
                if (tasks.Count == 0)
                    return "No tasks yet.";

                string output = "📋 YOUR TASKS:\n\n";

                for (int i = 0; i < tasks.Count; i++)
                {
                    output += $"{i + 1}. {tasks[i].Title} (Added: {tasks[i].Created})\n";
                }

                return output;
            }

            if (text.Contains("time"))
                return DateTime.Now.ToShortTimeString();

            if (text.Contains("date"))
                return DateTime.Now.ToShortDateString();

            return "Type 'help' for commands.";
        }
        private string ShowQuestion()
        {
            if (quizIndex >= quiz.Count)
            {
                quizMode = false;
                return $"Quiz finished! 🎉 Score: {score}/{quiz.Count}";
            }

            var q = quiz[quizIndex];

            if (q.Type == "tf")
            {
                return $"Q{quizIndex + 1}: {q.Question} (true/false)";
            }
            else
            {
                char letter = 'A';
                string options = "";

                foreach (var opt in q.Options)
                {
                    options += $"{letter}) {opt}\n";
                    letter++;
                }

                return $"Q{quizIndex + 1}: {q.Question}\n{options}";
            }
        }
        private string HandleQuiz(string input)
        {
            var q = quiz[quizIndex];
            string answer = input.Trim().ToLower();
            bool correct = false;

            if (q.Type == "tf")
            {
                bool userAnswer = answer.Contains("true") || answer == "t";
                correct = userAnswer == q.TrueFalseAnswer;
            }
            else
            {
                int index = -1;

                if (answer == "a")
                    index = 0;
                else if (answer == "b")
                    index = 1;
                else if (answer == "c")
                    index = 2;
                else if (answer == "d")
                    index = 3;

                if (index >= 0 && index < q.Options.Count)
                    correct = q.Options[index] == q.CorrectOption;
            }

            if (correct)
                score++;

            string result = correct ? "Correct ✔" : $"Wrong ❌ (Answer: {q.CorrectOption})";

            quizIndex++;

            // ✅ THIS is the important fix
            if (quizIndex < quiz.Count)
            {
                return result + "\n\n" + ShowQuestion();
            }
            else
            {
                quizMode = false;
                return $"{result}\n\n🎉 Quiz finished!\nFinal Score: {score}/{quiz.Count}";
            }
        }
    }
}
