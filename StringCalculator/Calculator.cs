using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace StringCalculator
{
    public class Calculator
    {
        public readonly string NullExceptionMessage = "Invalid Input: Input needed";
        public readonly string ParameterException = "Invalid Input: Can only be Signed Numeric characters";
        public readonly string DelimiterExceptionMessage = "Invalid Input: Delimiter can not be characters associated with numeric values";

        private RegexReplacer regexService = new RegexReplacer();

        public int Add(string numbers)
        {
            int result;
            try
            {
                if (string.IsNullOrEmpty(numbers))
                {
                    result = numbers != null ? 0 : throw new NullReferenceException(NullExceptionMessage);
                }
                else
                {
                    result = Parse(numbers).Where(x => x < 1001).Sum();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            return result;
        }

        private int[] Parse(string fullInput)
        {
            int[] numArray;
            string[] parseValue;
            string varyInput;

            try
            {
                fullInput.Replace(" ", string.Empty);

                if (fullInput.Length > 1 && regexService.isCustomDelimiter(fullInput))
                {
                    //int lineIndex = fullInput.IndexOf(@"\n") + 2;
                    //varyInput = fullInput.Substring(lineIndex, fullInput.Length - lineIndex);
                    //parseValue = new string[] { fullInput.Substring(2, 1) };

                    //if (parseValue.First().Equals("-") || int.TryParse(parseValue.First(), out int temp))
                    //    throw new FormatException(DelimiterExceptionMessage);

                    if (regexService.IsSingleHyphen(fullInput))
                        fullInput = regexService.ReplaceHyphen(fullInput);

                    if (regexService.isDefaultDelimiter(fullInput))
                        varyInput = regexService.ReplaceCustomDelimiter(fullInput);
                    else
                        varyInput = regexService.ReplaceCustomDelimiter_Expanded(fullInput);
                    parseValue = new string[] { "," };

                }
                else
                {
                    parseValue = new string[] { ",", @"\n" };
                    varyInput = fullInput;
                }

                string[] parsedNumbers;
                parsedNumbers = varyInput.Split(parseValue, StringSplitOptions.None);
                numArray = parsedNumbers.Where(x => x.Length > 0).Select(int.Parse).ToArray();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return numArray;
        }
    }
}
