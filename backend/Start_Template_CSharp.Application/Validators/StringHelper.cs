using System.Text.RegularExpressions;

namespace Start_Template_CSharp.Application.Validators;

public static class StringHelper
{
    public static string RemoveSpecialCharacters(string input)
    {
        // Шаблон [^\w\s] означает: любой символ, который не является словом (\w) или пробелом (\s)
        // Удаляем все спец. символы из строки.
        return Regex.Replace(input, @"[^\w\s\-_]", "",
            RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture,
            TimeSpan.FromSeconds(1));
    }
    public static string RemoveSpecCharEmail(string input)
    {
        // Шаблон [^\w\s] означает: любой символ, который не является словом (\w) или пробелом (\s)
        // Удаляем все спец. символы из строки.
        return Regex.Replace(input, @"[\w\.@-_]", "",
            RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture,
            TimeSpan.FromSeconds(1));
    }

    public static Regex  ValidationPhone() =>
          // new Regex(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");
           new Regex(@"^((\+7|7|8)+([0-9]){10})$",
               RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture,
               TimeSpan.FromSeconds(1));

}
