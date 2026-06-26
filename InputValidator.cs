// InputValidator.cs
// Validates and sanitises all user input before it reaches the ChatbotEngine.
// Keeping this logic in one place means the rest of the code always receives clean data.

using System;

namespace CybersecurityChatbot
{
    // Static utility class - no instance needed
    public static class InputValidator
    {
        // Words that mean the user wants to end the session
        private static readonly string[] ExitKeywords =
        {
            "exit", "quit", "bye", "goodbye", "close", "stop"
        };

        // Returns true if the input is not null, empty, or whitespace
        public static bool IsValidInput(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        // Returns true if the sanitised input exactly matches a recognised exit keyword
        public static bool IsExitCommand(string input)
        {
            if (!IsValidInput(input))
                return false;

            string sanitised = SanitizeInput(input);

            foreach (string keyword in ExitKeywords)
            {
                if (sanitised == keyword)
                    return true;
            }

            return false;
        }

        // Trims whitespace and converts to lowercase for consistent keyword matching
        // e.g. "  Tell Me About PHISHING  " becomes "tell me about phishing"
        public static string SanitizeInput(string input)
        {
            return input.Trim().ToLower();
        }

        // Title-cases a name so it displays neatly
        // e.g. "john doe" becomes "John Doe"
        public static string SanitizeName(string input)
        {
            string trimmed = input.Trim();

            if (trimmed.Length == 0)
                return trimmed;

            string[] words = trimmed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }

            return string.Join(" ", words);
        }
    }
}
