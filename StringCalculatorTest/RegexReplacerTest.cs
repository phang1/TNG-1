using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringCalculator;
using System;

using System.Text;

namespace StringCalculatorTest
{
    [TestClass]
    public class RegexReplacerTest
    {
        RegexReplacer RegexReplacer = new RegexReplacer();

        [TestMethod]
        public void SingleHyphenTest()
        {
            string input = @"//[-][pa]\npa10-20-30";
            bool result = RegexReplacer.IsSingleHyphen(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NoSingleHyphenTest()
        {

            string input = @"//[a][pa]\npa10a20a30";
            bool result = RegexReplacer.IsSingleHyphen(input);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReplaceHyphen_Vanilla()
        {
            string input = @"//[--]\n--10---20--30";
            string expectedResult = @",10,-20,30";

            string result = RegexReplacer.Replace(input);
            Assert.IsTrue(result.Equals(expectedResult));
        }

        [TestMethod]
        public void Replace_Multi()
        {
            string input = @"//[--][;;]\n--10---20--30;;20";
            string expectedResult = @",10,-20,30,20";

            string result = RegexReplacer.Replace(input);
            Assert.IsTrue(result.Equals(expectedResult));
        }

        [TestMethod]
        public void ReplaceExpanded_ContainingDelimiterExist()
        {
            string input = @"//[---][--]\n---10---20---30---20";
            string expectedResult = @",10,20,30,20";

            string result = RegexReplacer.Replace(input);
            Assert.IsTrue(result.Equals(expectedResult));
        }

        [TestMethod]
        public void ReplaceExpanded_AddContainingDelimiter()
        {
            string input = @"//[--][---]\n---10---20---30---20";
            string expectedResult = @",10,20,30,20";

            string result = RegexReplacer.Replace(input);
            Assert.IsTrue(result.Equals(expectedResult));
        }

        [TestMethod]
        public void ReplaceExpanded_HyphenPartialSubsets_WithNegative()
        {
            string input = @"//[--][---]\n---10---20---30----20";
            string expectedResult = @",10,20,30,-20";

            string result = RegexReplacer.Replace(input);
            Assert.IsTrue(result.Equals(expectedResult));
        }

        //[TestMethod]
        //public void ReplaceHyphen_HyphenInvalidPartialSubsets()
        //{
        //    string input = @"//[--][---]\n--10--20---30---20";
        //    string expectedResult = @",10,-20,30,20";

        //    string result = RegexReplacer.ReplaceHyphen(input);
        //    Assert.IsTrue(result.Equals(expectedResult));
        //}

        [TestMethod]
        public void ReplaceExpanded_Vanilla()
        {
            string input = @"//[a]\na10a20a30";
            string expectedResult = @",10,20,30";

            string result = RegexReplacer.Replace(input);
            Assert.IsTrue(result.Equals(expectedResult));
        }

        [TestMethod]
        public void ReplaceExpanded_Quotations()
        {
            string input = @"//[""]\n""10""20""30";
            string expectedResult = @",10,20,30";

            string result = RegexReplacer.Replace(input);
            Assert.IsTrue(result.Equals(expectedResult));
        }

        [TestMethod]
        public void ReplaceExpanded_DelimiterPartialSubsets()
        {
            string input = @"//[a][pa]\npa10a20a30";
            string expectedResult = @",10a20a30";

            string result = RegexReplacer.Replace(input);
            Assert.IsTrue(result.Equals(expectedResult));
        }

        [TestMethod]
        public void ReplaceCustomDelimiter_Default()
        {
            string input = @"//a\na10a20a30";
            string expectedResult = @",10,20,30";

            string result = RegexReplacer.Replace(input);
            Assert.IsTrue(result.Equals(expectedResult));
        }

        [TestMethod]
        public void ReplaceDefaultCustomDelimiter_Numeric()
        {
            string input = @"//1\n110120130";
            Assert.ThrowsException<ArgumentException>(() => RegexReplacer.Replace(input));
        }

        [TestMethod]
        public void ReplaceExpandedCustomDelimiter_Numeric()
        {
            string input = @"//[1][-]\n1101201-30";
            Assert.ThrowsException<ArgumentException>(() => RegexReplacer.Replace(input));
        }

        [TestMethod]
        public void IsCustomDelimiter_Positive()
        {
            string input = @"//;\n10;20;30";
            Assert.IsTrue(RegexReplacer.isCustomDelimiter(input));
        }

        [TestMethod]
        public void IsCustomDelimiter_Negative()
        {
            string input = "10,20,30";
            Assert.IsFalse(RegexReplacer.isCustomDelimiter(input));
        }

        [TestMethod]
        public void ReplaceSingleHyphen_DefaultSyntax()
        {
            string input = @"//-\n-10-20-30";
            string expectedResult = @"//,\n,10,20,30";
            string result = RegexReplacer.ReplaceHyphen(input);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ReplaceSingleHyphen_ExpandedSyntax()
        {
            string input = @"//[-]\n-10-20-30";
            string expectedResult = @"//[,]\n,10,20,30";
            string result = RegexReplacer.ReplaceHyphen(input);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
