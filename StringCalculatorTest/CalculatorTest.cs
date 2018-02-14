using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringCalculator;
using System;

namespace StringCalculatorTest
{
    [TestClass]
    public class CalculatorTest
    {
        static Calculator calc;
        // BASE: [\/]{2}.*[\\n]*
        // CONTAINS HYPHEN: [\/]{2}[^-]*[\\n]*

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            calc = new Calculator();
        }

        #region Old Tests

        [TestMethod]
        public void AddNullParameter()
        {
            string input = null;
            Assert.ThrowsException<NullReferenceException>(() => calc.Add(input));
        }

        [TestMethod]
        public void AddEmptyString()
        {
            string input = "";
            int result = calc.Add(input);
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void AddSingleValue()
        {
            string input = "1";
            int result = calc.Add(input);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void AddWithEmptyValue_NewLineDelimited()
        {
            string input = @"10\n,10\n\n10";
            int result = calc.Add(input);
            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void AddWithEmptyValue_CommaDelimited()
        {
            string input = "10,10,,10";
            int result = calc.Add(input);
            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void AddManyValues_CommaDelimited()
        {
            string input = "1,2,3,4,5";
            int result = calc.Add(input);
            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void AddManyValues_NewLineDelimited()
        {
            string input = @"1\n2\n3\n4\n5";
            int result = calc.Add(input);
            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void AddManyValues_NewLineAndCommaDelimited()
        {
            string input = @"1,2\n3\n4,5";
            int result = calc.Add(input);
            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void AddCrazyParameters()
        {
            string input = "asdf;lkjai1 , asld;fkjso10";
            Assert.ThrowsException<FormatException>(() => calc.Add(input));
        }

        [TestMethod]
        public void AddWithZeros()
        {
            string input = "0,1";
            int result = calc.Add(input);
            Assert.AreEqual(1, result);

            input = "0,0";
            result = calc.Add(input);
            Assert.AreEqual(0, result);

            input = @"0,0\n0";
            result = calc.Add(input);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void AddWithNegatives()
        {
            string input = "-10,-10";
            int result = calc.Add(input);
            Assert.AreEqual(-20, result);

            input = "10,-10";
            result = calc.Add(input);
            Assert.AreEqual(0, result);

            input = @"-10\n10,10";
            result = calc.Add(input);
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void AddWithNegatives_BadFormat()
        {
            string input = "10,10-10";
            Assert.ThrowsException<FormatException>(() => calc.Add(input));
        }

        #endregion

        #region Assignemnt 3 Tests

        [TestMethod]
        public void AddCustomDelimiter_SpecialCharacter()
        {
            string input = @"//;\n10;20;30";
            int result = calc.Add(input);
            Assert.AreEqual(60, result);
        }

        [TestMethod]
        public void AddCustomDelimiter_AlphabeticCharacter()
        {
            string input = @"//a\n10a20a30";
            int result = calc.Add(input);
            Assert.AreEqual(60, result);
        }

        [TestMethod]
        public void AddCustomDelimiter_Hyphen()
        {
            string input = @"//-\n-10-20-30";
            int result = calc.Add(input);
            Assert.AreEqual(60, result);
        }

        //[TestMethod]
        //public void AddCustomDelimiter_MultiCharacter()
        //{
        //    string input = "//nn\n-10-20-30";
        //    Assert.ThrowsException<FormatException>(() => calc.Add(input));
        //}

        [TestMethod]
        public void AddCustomDelimiter_SingleLeadingBackslash()
        {
            string input = @"/;\n10;20;30";
            Assert.ThrowsException<FormatException>(() => calc.Add(input));
        }

        [TestMethod]
        public void AddCustomDelimiter_NoLeadingBackslash()
        {
            string input = @";\n10;20;30";
            Assert.ThrowsException<FormatException>(() => calc.Add(input));
        }

        [TestMethod]
        public void AddEqualToThousand_DefaultDelimiter()
        {
            string input = "10,100,1000";
            int result = calc.Add(input);
            Assert.AreEqual(1110, result);
        }

        [TestMethod]
        public void AddEqualToThousand_CustomDelimiter()
        {
            string input = @"//;\n10;100;1000";
            int result = calc.Add(input);
            Assert.AreEqual(1110, result);
        }

        [TestMethod]
        public void AddGreaterThanThousand_DefaultDelimiter()
        {
            string input = "10,100,2000";
            int result = calc.Add(input);
            Assert.AreEqual(110, result);
        }

        [TestMethod]
        public void AddGreaterThanThousand_CustomDelimiter()
        {
            string input = @"//;\n10;100;2000";
            int result = calc.Add(input);
            Assert.AreEqual(110, result);
        }

        [TestMethod]
        public void AddLessThanNegativeThousand_DefaultDelimiter()
        {
            string input = @"//;\n10;100;-2000";
            int result = calc.Add(input);
            Assert.AreEqual(-1890, result);
        }

        [TestMethod]
        public void AddLessThanNegativeThousand_CustomDelimiter()
        {
            string input = @"//;\n10;100;-2000";
            int result = calc.Add(input);
            Assert.AreEqual(-1890, result);
        }

        #endregion

        #region Assignment 4 Tests

        [TestMethod]
        public void AddCustomDelimiter_MultiCharacter()
        {
            string input = @"//[nn]\nnn10nn20nn30";
            int result = calc.Add(input);
            Assert.AreEqual(60, result);
        }

        [TestMethod]
        public void AddMultipleCustomDelimiters()
        {
            string input = @"//[;][-]\n10-20;30;40-50";
            int result = calc.Add(input);
            Assert.AreEqual(150, result);
        }

        [TestMethod]
        public void AddCustomDelimiter_HyphenBraces_NegativeNumber()
        {
            string input = @"//[-]\n10--10--100-100";
            int result = calc.Add(input);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void AddCustomDelimiter_HyphenNoBraces_NegativeNumber()
        {
            string input = @"//-\n10--10--100-100";
            int result = calc.Add(input);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void AddCustomDelimiter_MultiHyphenNegativeNumber()
        {
            string input = @"//[---]\n10----10---100----100";
            int result = calc.Add(input);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void AddCustomDelimiter_MultiDelimiter_IncludingHyphen()
        {
            string input = @"//[;;][-]\n10;;20-30--60";
            int result = calc.Add(input);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void AddCustomDelimiterExpanded_NumericCharacter()
        {
            string input = @"//[-][10][1]\n10120130";
            Assert.ThrowsException<ArgumentException>(() => calc.Add(input));
        }

        #endregion
    }
}
