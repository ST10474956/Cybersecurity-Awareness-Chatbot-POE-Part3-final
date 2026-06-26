using System.Collections.Generic;

public class QuizManager
{
    public List<Question> Questions = new List<Question>()
    {
        new Question{Text="What is phishing?", Options=new[]{"Scam","Game","Tool","Browser"}, CorrectIndex=0},
        new Question{Text="Strong password?", Options=new[]{"1234","password","P@ss!2024","abc"}, CorrectIndex=2},
        new Question{Text="Share password?", Options=new[]{"Yes","No"}, CorrectIndex=1},
        new Question{Text="2FA purpose?", Options=new[]{"Extra security","Faster login"}, CorrectIndex=0},
        new Question{Text="Safe link?", Options=new[]{"Unknown link","Verified URL"}, CorrectIndex=1},
        new Question{Text="VPN use?", Options=new[]{"Encrypt traffic","Slow internet"}, CorrectIndex=0},
        new Question{Text="Update software?", Options=new[]{"Ignore","Update"}, CorrectIndex=1},
        new Question{Text="Public WiFi safe?", Options=new[]{"Yes","No"}, CorrectIndex=1},
        new Question{Text="Antivirus?", Options=new[]{"Protects","Harms"}, CorrectIndex=0},
        new Question{Text="Backup data?", Options=new[]{"Yes","No"}, CorrectIndex=0},
        new Question{Text="Social engineering?", Options=new[]{"Manipulation","Coding"}, CorrectIndex=0},
        new Question{Text="HTTPS means?", Options=new[]{"Secure","Fast"}, CorrectIndex=0}
    };
}
