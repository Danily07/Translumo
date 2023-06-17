using System.Text.RegularExpressions;

namespace Translumo.Utils
{
    public static class RegexHelper
    {
        public static string Match(string input, string regex, string? namedGroup = null)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var matchResult = Regex.Match(input, regex);
            if (!matchResult.Success)
            {
                return string.Empty;
            }

            return string.IsNullOrEmpty(namedGroup) ? matchResult.Value : matchResult.Groups[namedGroup].Value;
        }
    }
}
