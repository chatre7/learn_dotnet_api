using System;
using System.Text.RegularExpressions;
using Application.Utilities;

class Program
{
    static void Main(string[] args)
    {
        var input = "SELECT * FROM users; DROP TABLE users;";
        Console.WriteLine($"Input: {input}");
        
        var result = InputSanitizer.SanitizeString(input);
        Console.WriteLine($"Result: '{result}'");
        Console.WriteLine($"Expected: ' * FROM users;  TABLE users;'");
        Console.WriteLine($"Match: {result == " * FROM users;  TABLE users;"}");
        
        // Let's also test step by step
        var step1 = Regex.Replace(input, @"(?i)<script[^>]*>.*?</script>", string.Empty, RegexOptions.Singleline);
        Console.WriteLine($"After script removal: '{step1}'");
        
        var step2 = Regex.Replace(step1, @"<[^>]*>", string.Empty);
        Console.WriteLine($"After HTML tag removal: '{step2}'");
        
        var step3 = Regex.Replace(step2, @"(?i)\bselect\b", " ", RegexOptions.IgnoreCase);
        Console.WriteLine($"After SELECT replacement: '{step3}'");
        
        var step4 = Regex.Replace(step3, @"(?i)\bdrop\b", " ", RegexOptions.IgnoreCase);
        Console.WriteLine($"After DROP replacement: '{step4}'");
        
        var step5 = Regex.Replace(step4, @"\s+", " ");
        Console.WriteLine($"After space cleanup: '{step5}'");
        
        Console.WriteLine($"Final result: '{step5.Trim()}'");
    }
}