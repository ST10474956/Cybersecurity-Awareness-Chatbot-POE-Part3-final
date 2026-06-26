using System.Text.RegularExpressions;

public class ChatbotEngine
{
    public string ProcessInput(string input)
    {
        input = input.ToLower();

        if (Regex.IsMatch(input, "add.*task|remind.*me|set.*reminder"))
            return "TASK";

        if (Regex.IsMatch(input, "quiz|game|test"))
            return "QUIZ";

        if (Regex.IsMatch(input, "log|history|what.*done"))
            return "LOG";

        return "UNKNOWN";
    }
}
