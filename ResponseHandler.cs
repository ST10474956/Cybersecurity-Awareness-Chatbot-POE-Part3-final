// ResponseHandler.cs
// Stores all chatbot response text and provides three lookup methods:
//   GetResponse()        - matches a keyword in the input, returns a random response
//   GetFollowUp()        - returns another random response for the current topic
//   GetSentimentPrefix() - returns an empathetic phrase to prepend when a mood is detected
//
// Responses use Dictionary<string, List<string>> so each topic has multiple options
// and one is picked randomly each time - satisfying the Random Responses requirement.

using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class ResponseHandler
    {
        // Single shared Random instance - avoids the repeated-seed problem
        private readonly Random _rng = new Random();

        // Each topic keyword maps to a list of responses.
        // One is selected randomly each time the topic is matched.
        private readonly Dictionary<string, List<string>> _topicResponses
            = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["phishing"] = new List<string>
            {
                "PHISHING AWARENESS\n" +
                "Phishing is when cybercriminals send fake emails or messages pretending\n" +
                "to be trusted organisations (e.g. your bank, SARS, or SASSA) to steal\n" +
                "your personal information or login credentials.\n\n" +
                "  - Never click links in unsolicited emails - go directly to the website\n" +
                "  - Check the sender's full email address carefully\n" +
                "  - Look for spelling mistakes and urgency (e.g. 'Your account will be closed!')\n" +
                "  - Never provide your password, OTP, or ID number via email\n" +
                "  - Report phishing emails to your IT department or bank",

                "PHISHING TIP\n" +
                "Be cautious of emails asking for personal information.\n" +
                "Scammers often disguise themselves as trusted organisations.\n\n" +
                "  - Hover over links to preview the actual URL before clicking\n" +
                "  - A padlock icon does NOT guarantee a site is safe\n" +
                "  - When in doubt, go directly to the official website",

                "PHISHING TIP\n" +
                "Spear phishing targets you personally using info gathered from social media.\n\n" +
                "  - Be suspicious of emails that use your name and create urgency\n" +
                "  - Verify unexpected requests from colleagues by calling them directly\n" +
                "  - Never enter login credentials on a page you reached via an email link"
            },

            ["password"] = new List<string>
            {
                "PASSWORD SAFETY\n" +
                "Weak passwords are one of the leading causes of account breaches in South Africa.\n\n" +
                "  - Use at least 12 characters combining uppercase, lowercase, numbers, and symbols\n" +
                "  - Never reuse the same password across multiple sites\n" +
                "  - Use a reputable password manager (e.g. Bitwarden, 1Password)\n" +
                "  - Change passwords immediately if you suspect a breach\n" +
                "  - Never share your password with anyone, not even IT staff",

                "PASSWORD TIP\n" +
                "Make sure to use strong, unique passwords for each account.\n" +
                "Avoid using personal details like your name or birthdate.\n\n" +
                "  - A passphrase like 'PurpleFish!RunsFast99' is strong and memorable\n" +
                "  - Enable two-factor authentication alongside your password",

                "PASSWORD TIP\n" +
                "A password manager generates and stores complex passwords securely.\n\n" +
                "  - Bitwarden is free, open-source, and highly trusted\n" +
                "  - Never write passwords on sticky notes or store them unencrypted\n" +
                "  - Check haveibeenpwned.com to see if your passwords have been leaked"
            },

            ["scam"] = new List<string>
            {
                "SCAM AWARENESS\n" +
                "South Africa is among the top targets for online scams globally.\n" +
                "Common scams include advance-fee fraud, fake job offers, and vishing.\n\n" +
                "  - If it sounds too good to be true, it almost certainly is\n" +
                "  - Never pay upfront fees for prizes or loans\n" +
                "  - Report scams to the SA Fraud Prevention Services: 0860 101 248\n" +
                "  - Check the Hawks (DPCI) website for known scam alerts",

                "SCAM TIP\n" +
                "Scammers create urgency so you do not have time to think - slow down.\n\n" +
                "  - Never send money or gift cards to someone you have only met online\n" +
                "  - Verify job offers and opportunities independently before responding\n" +
                "  - Screenshot and report suspicious messages to your bank immediately",

                "SCAM TIP\n" +
                "Legitimate organisations will never ask you to pay with gift cards.\n\n" +
                "  - If someone calls claiming to be from your bank or SARS, hang up\n" +
                "    and call back on the official number to verify\n" +
                "  - Romance scams are on the rise in SA - be cautious of online relationships\n" +
                "    that quickly ask for money"
            },

            ["privacy"] = new List<string>
            {
                "DATA PRIVACY\n" +
                "South Africa's Protection of Personal Information Act (POPIA) protects\n" +
                "your personal data. Know your rights and protect your information.\n\n" +
                "  - Read privacy policies before accepting apps or services\n" +
                "  - Regularly review and limit app permissions on your phone\n" +
                "  - Minimise how much personal info you share on social media\n" +
                "  - Use privacy-focused search engines like DuckDuckGo or Brave",

                "PRIVACY TIP\n" +
                "Review your social media privacy settings regularly.\n\n" +
                "  - Cybercriminals can piece together your details for targeted attacks\n" +
                "  - Use a VPN when connecting to public Wi-Fi to protect your data\n" +
                "  - Opt out of marketing data sharing where possible",

                "PRIVACY TIP\n" +
                "Every app you install potentially has access to your data.\n\n" +
                "  - Audit which third-party apps have access to your accounts\n" +
                "  - Revoke permissions for apps you no longer use\n" +
                "  - Check your Google Activity settings to see what data is being collected"
            },

            ["malware"] = new List<string>
            {
                "MALWARE\n" +
                "Malware includes viruses, trojans, spyware, adware, and ransomware -\n" +
                "all designed to damage or take control of your device.\n\n" +
                "  - Install reputable antivirus software (e.g. Malwarebytes, Kaspersky)\n" +
                "  - Never download software from unofficial or unknown websites\n" +
                "  - Keep your operating system and applications fully updated\n" +
                "  - Be cautious of USB drives from unknown sources",

                "MALWARE TIP\n" +
                "Keep your antivirus software up to date to protect against the latest threats.\n\n" +
                "  - Avoid free software bundled with toolbars or extras - these can be spyware\n" +
                "  - Always download software from the official developer website",

                "MALWARE TIP\n" +
                "Scan external USB drives before opening files - they can carry malware.\n\n" +
                "  - Disable AutoRun for removable drives in Windows settings\n" +
                "  - If your device feels slow or behaves strangely, run a full malware scan"
            },

            ["ransomware"] = new List<string>
            {
                "RANSOMWARE\n" +
                "Ransomware encrypts your files and demands payment for the decryption key.\n" +
                "South Africa has seen major attacks on hospitals and municipalities.\n\n" +
                "  - Back up your data regularly (3-2-1 rule: 3 copies, 2 media types, 1 offsite)\n" +
                "  - Never open unexpected email attachments, especially .zip or .exe files\n" +
                "  - Do NOT pay the ransom - there is no guarantee you will get your files back\n" +
                "  - Report incidents to the SA Cybercrime Hub or SAPS",

                "RANSOMWARE TIP\n" +
                "The best defence against ransomware is maintaining regular, offline backups.\n\n" +
                "  - An external hard drive kept disconnected cannot be encrypted by ransomware\n" +
                "  - Cloud backups with version history (OneDrive, Google Drive) also help recovery",

                "RANSOMWARE TIP\n" +
                "Ransomware often enters through phishing emails or unpatched software.\n\n" +
                "  - Enable automatic Windows updates to close known vulnerabilities\n" +
                "  - Disable macros in Office documents received from unknown senders"
            },

            ["social engineering"] = new List<string>
            {
                "SOCIAL ENGINEERING\n" +
                "Social engineering manipulates people into revealing confidential information\n" +
                "by exploiting trust, fear, or urgency.\n\n" +
                "  - Be sceptical of unexpected calls or emails claiming to be IT or your bank\n" +
                "  - Verify a person's identity before sharing any information\n" +
                "  - Legitimate organisations will NEVER ask for your password or OTP\n" +
                "  - Hang up and call back the official number to verify",

                "SOCIAL ENGINEERING TIP\n" +
                "Pretexting is when an attacker creates a fake scenario to extract information.\n\n" +
                "  - Even if a caller knows your personal details, that does not mean they are legitimate\n" +
                "  - Never be rushed into sharing sensitive information",

                "SOCIAL ENGINEERING TIP\n" +
                "Baiting uses items like infected USB drives left in public places.\n\n" +
                "  - Never plug in a USB drive you found or received unexpectedly\n" +
                "  - Curiosity is exactly what attackers rely on - do not take the bait"
            },

            ["2fa"] = new List<string>
            {
                "TWO-FACTOR AUTHENTICATION (2FA)\n" +
                "2FA adds a second verification step beyond your password. Even if your\n" +
                "password is stolen, an attacker still cannot access your account.\n\n" +
                "  - Enable 2FA on all important accounts (email, banking, social media)\n" +
                "  - Prefer app-based 2FA (Google Authenticator, Microsoft Authenticator)\n" +
                "  - Avoid SMS-based 2FA where possible - SIM-swap fraud is common in SA\n" +
                "  - Never share your 2FA code with anyone",

                "2FA TIP\n" +
                "SIM-swap fraud is common in South Africa - attackers transfer your number\n" +
                "to their SIM card to intercept your SMS one-time passwords.\n\n" +
                "  - Use an authenticator app instead of SMS codes wherever possible\n" +
                "  - Ask your network provider to add a PIN to your account",

                "2FA TIP\n" +
                "Passkeys are a modern, phishing-resistant alternative to passwords and OTPs.\n\n" +
                "  - Google, Apple, and Microsoft accounts now support passkeys\n" +
                "  - Passkeys are stored on your device and cannot be stolen via phishing"
            },

            ["vpn"] = new List<string>
            {
                "VPN (VIRTUAL PRIVATE NETWORK)\n" +
                "A VPN encrypts your internet traffic, making it much harder for\n" +
                "cybercriminals to intercept your data.\n\n" +
                "  - Always use a VPN when connecting to public Wi-Fi\n" +
                "  - Choose reputable paid providers (e.g. ProtonVPN, NordVPN)\n" +
                "  - Avoid free VPNs - they often sell your browsing data\n" +
                "  - A VPN reduces your exposure but does not make you fully anonymous",

                "VPN TIP\n" +
                "Always use a VPN on public Wi-Fi networks like coffee shops or airports.\n\n" +
                "  - Without a VPN, anyone on the same network can intercept your traffic\n" +
                "  - ProtonVPN offers a free tier with no data limits and a no-logs policy",

                "VPN TIP\n" +
                "A VPN encrypts your connection but does not protect against phishing.\n\n" +
                "  - Think of a VPN as one layer of protection, not a complete solution\n" +
                "  - Always combine VPN use with strong passwords and updated software"
            },

            ["wifi"] = new List<string>
            {
                "WI-FI SECURITY\n" +
                "Public Wi-Fi hotspots are a prime target for attacks where criminals\n" +
                "intercept your data.\n\n" +
                "  - Avoid banking or sensitive logins on public Wi-Fi\n" +
                "  - Use a VPN whenever connecting to public networks\n" +
                "  - Ensure your home router uses WPA3 or at minimum WPA2 encryption\n" +
                "  - Change your router's default username and password",

                "WI-FI TIP\n" +
                "Keep your home router firmware updated - manufacturers patch security flaws.\n\n" +
                "  - Use a guest network for smart home devices\n" +
                "  - Disable WPS on your router - it has known vulnerabilities",

                "WI-FI TIP\n" +
                "Fake hotspots with names like 'FreeAirportWifi' are used to steal your data.\n\n" +
                "  - Always verify the exact hotspot name with staff before connecting\n" +
                "  - Use your mobile data instead of public Wi-Fi for sensitive transactions"
            },

            ["identity theft"] = new List<string>
            {
                "IDENTITY THEFT\n" +
                "Identity theft occurs when someone uses your personal information\n" +
                "without your consent to commit fraud.\n\n" +
                "  - Never share your ID number unless absolutely necessary\n" +
                "  - Shred physical documents containing personal information\n" +
                "  - Monitor your credit report regularly (TransUnion, Experian)\n" +
                "  - Be careful what you post on social media",

                "IDENTITY THEFT TIP\n" +
                "South African ID numbers contain your date of birth and gender - guard them.\n\n" +
                "  - Check your credit report yearly for accounts you did not open\n" +
                "  - If your ID is lost or stolen, report it to Home Affairs immediately",

                "IDENTITY THEFT TIP\n" +
                "Data breaches at large companies can expose your details to criminals.\n\n" +
                "  - Check haveibeenpwned.com to see if your email appeared in a breach\n" +
                "  - Change passwords and enable 2FA on any accounts flagged in a breach"
            },

            ["browsing"] = new List<string>
            {
                "SAFE BROWSING\n" +
                "Your browser is your main window to the internet - keep it secure.\n\n" +
                "  - Always check for HTTPS before entering personal info on a site\n" +
                "  - Keep your browser and extensions up to date\n" +
                "  - Clear cookies and browser history regularly\n" +
                "  - Use privacy-focused search engines like DuckDuckGo",

                "SAFE BROWSING TIP\n" +
                "Browser extensions can be hijacked or sold to malicious parties.\n\n" +
                "  - Only install extensions from the official browser store\n" +
                "  - Remove extensions you do not recognise or no longer use",

                "SAFE BROWSING TIP\n" +
                "Avoid downloading files from unknown sources - even PDFs can carry malware.\n\n" +
                "  - Verify URLs carefully before entering login details\n" +
                "  - Log out of websites when done, especially on shared computers"
            },

            ["popia"] = new List<string>
            {
                "POPIA - PROTECTION OF PERSONAL INFORMATION ACT\n" +
                "POPIA is South Africa's data privacy law, similar to GDPR in Europe.\n" +
                "It governs how organisations may collect, store, and use your personal data.\n\n" +
                "  - You have the right to know what data is held about you\n" +
                "  - You can request correction or deletion of your data\n" +
                "  - Organisations must notify you if your data is breached\n" +
                "  - Report violations to the Information Regulator: inforeg.org.za",

                "POPIA TIP\n" +
                "Under POPIA, companies must get your consent before collecting personal data.\n\n" +
                "  - You can withdraw consent at any time and request deletion of your data\n" +
                "  - Submit POPIA complaints to inforeg.org.za"
            }
        };

        // Single fixed responses for general queries that do not need random selection
        private readonly Dictionary<string, string> _fixedResponses
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["how are you"] =
                "I am running smoothly and always vigilant! How can I help you stay safe today?",

            ["what is your purpose"] =
                "My purpose is to educate South African citizens about cybersecurity threats\n" +
                "and best practices. Ask me about phishing, passwords, malware, and more.",

            ["what can i ask"] =
                "You can ask me about:\n\n" +
                "  - Phishing              - Password Safety\n" +
                "  - Safe Browsing         - Malware and Ransomware\n" +
                "  - Social Engineering    - Two-Factor Authentication (2FA)\n" +
                "  - VPN                   - Data Privacy and POPIA\n" +
                "  - Wi-Fi Security        - Identity Theft\n" +
                "  - Scam Awareness\n\n" +
                "Just type any of these topics to learn more!",

            ["help"] =
                "HELP MENU - Topics I can assist with:\n\n" +
                "  phishing           password\n" +
                "  safe browsing      malware\n" +
                "  ransomware         social engineering\n" +
                "  2fa                vpn\n" +
                "  privacy            wi-fi\n" +
                "  identity theft     scam\n" +
                "  popia              what can i ask\n\n" +
                "Type 'exit' at any time to quit."
        };

        // Empathetic phrases prepended to topic responses when a sentiment is detected.
        // Multiple options per sentiment - one is chosen randomly for variety.
        private readonly Dictionary<string, List<string>> _sentimentPrefixes
            = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["worried"] = new List<string>
            {
                "I understand your concern - that is a completely valid feeling. ",
                "It is okay to feel worried about this. Let me reassure you: ",
                "Your concern shows you care about staying safe. Here is what you should know: "
            },
            ["frustrated"] = new List<string>
            {
                "I can hear your frustration. Let me try to make this clearer: ",
                "That is a fair point to be frustrated about. Here is a simpler way to look at it: ",
                "I am sorry this feels overwhelming. Let us break it down together: "
            },
            ["curious"] = new List<string>
            {
                "Great question! I love your curiosity. ",
                "Wonderful - curiosity is the first step to staying safe online! ",
                "That is a really interesting thing to explore. "
            }
        };

        // Finds a matching keyword in the input and returns a random response for that topic.
        // Longer keys are sorted first so "social engineering" matches before "scam" etc.
        public string GetResponse(string sanitisedInput, string userName)
        {
            // Check fixed responses first (help, greetings, purpose)
            foreach (var entry in _fixedResponses)
            {
                if (sanitisedInput.Contains(entry.Key))
                    return entry.Value;
            }

            // Sort topic keys longest-first to avoid partial matches
            List<string> keys = new List<string>(_topicResponses.Keys);
            keys.Sort((a, b) => b.Length.CompareTo(a.Length));

            foreach (string key in keys)
            {
                if (sanitisedInput.Contains(key))
                    return GetRandom(_topicResponses[key]);
            }

            // Nothing matched - return the required default error message
            return "I'm not sure I understand. Can you try rephrasing?";
        }

        // Returns a random response for the current topic when the user asks for more
        public string GetFollowUp(string currentTopic, string userName)
        {
            if (!string.IsNullOrEmpty(currentTopic) && _topicResponses.ContainsKey(currentTopic))
                return GetRandom(_topicResponses[currentTopic]);

            // No active topic yet - return a general tip
            return "Here is a general tip, " + userName + ": always keep your software and " +
                   "operating system up to date. Many attacks exploit unpatched vulnerabilities.";
        }

        // Returns a random empathetic prefix phrase for the given sentiment word
        public string GetSentimentPrefix(string sentimentWord)
        {
            if (_sentimentPrefixes.ContainsKey(sentimentWord))
                return GetRandom(_sentimentPrefixes[sentimentWord]);

            return string.Empty;
        }

        // Picks a random item from a list using the shared Random instance
        private string GetRandom(List<string> list)
        {
            return list[_rng.Next(list.Count)];
        }
    }
}
