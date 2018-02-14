using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace StringCalculator
{
    public class RegexReplacer
    {
        #region Regex Patterns

        string DefaultDelimiterPattern = @"[\/]{2}[^\d-]\\n";
        string DefaultDelimiterNumericPattern = @"[\/]{2}[\d]\\n";
        string ExpandedDelimiterPattern = @"[[][^\[\]]*[]]";
        string HyphenNoBracePattern = @"[\/]{2}[-]\\n";
        string HyphenBracesPattern = @"[\/]{2}.*[[][-][]].*";
        string TwoLeadingBackslashPattern = @"^[\/]{2}";

        #endregion

        #region Public Methods

        public bool IsSingleHyphen(string inputString)
        {
            bool isHyphen = false;

            Regex hyphenRegex = new Regex(HyphenBracesPattern);
            Regex hyphenNoBrace = new Regex(HyphenNoBracePattern);

            Match matchBrace = hyphenRegex.Match(inputString);
            Match matchNoBrace = hyphenNoBrace.Match(inputString);
            isHyphen = matchNoBrace.Success || matchBrace.Success;

            return isHyphen;
        }

        public string ReplaceHyphen(string inputString)
        {
            string hyphenNoTrailingNumberPattern = @"(?<=[^-])-";

            string returnString = Regex.Replace(inputString, hyphenNoTrailingNumberPattern, ",");
            return returnString;
        }

        public string Replace(string inputString)
        {
            string returnString;

            if (isDefaultDelimiter(inputString))
                returnString = ReplaceCustomDelimiter(inputString);
            else
                returnString = ReplaceCustomDelimiter_Expanded(inputString);

            return returnString;
        }

        public bool isCustomDelimiter(string inputString)
        {
            bool leadingBackslash = Regex.Match(inputString, TwoLeadingBackslashPattern).Success;

            return leadingBackslash;
        }


        #endregion

        #region Private Methods

        private string ReplaceCustomDelimiter_Expanded(string inputString)
        {
            string returnString = (Regex.Split(inputString, @"\\n"))[1] ?? throw new FormatException();

            List<string> delimiterList = new List<string>();

            Regex delimiterRegex = new Regex(ExpandedDelimiterPattern);
            Match m = delimiterRegex.Match(inputString);

            while (m.Success)
            {
                IEnumerable matches = m.Captures;
                foreach (var match in matches)
                {
                    string delimiter = ((Match)match).Value;
                    delimiter = delimiter.Replace("[", string.Empty);
                    delimiter = delimiter.Replace("]", string.Empty);

                    if (delimiterList.Any(s => delimiter.Contains(s)))
                    {
                        delimiterList.Remove(
                            delimiterList
                            .Where(s => delimiter.Contains(s))
                            .First());
                    }
                    else if (delimiterList.Any(s => s.Contains(delimiter)))
                        break;

                    delimiterList.Add(delimiter);
                }
                m = m.NextMatch();
            }

            foreach(string delim in delimiterList)
            {
                if (isDelimiterNonNumeric(delim))
                    throw new ArgumentException();
                returnString = returnString.Replace(delim, ",");
            }

            return returnString;
        }

        private string ReplaceCustomDelimiter(string inputString)
        {
            string returnString = (Regex.Split(inputString, @"\\n"))[1] ?? throw new FormatException();

            Regex delimiterRegex = new Regex(DefaultDelimiterPattern);
            string delimiter = delimiterRegex.Match(inputString).Value;
            delimiter = delimiter.Substring(2, 1);
            returnString = returnString.Replace(delimiter, ",");

            return returnString;
        }

        public bool isDefaultDelimiter(string inputString)
        {
            if (Regex.Match(inputString, DefaultDelimiterNumericPattern).Success)
                throw new ArgumentException();

            return Regex.Match(inputString, DefaultDelimiterPattern).Success;
        }

        private bool isDelimiterNonNumeric(string inputString)
        {
            return int.TryParse(inputString, out int value);
        }

        #endregion
    }
}
