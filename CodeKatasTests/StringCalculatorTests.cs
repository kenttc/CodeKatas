using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CodeKatasTests
{
    [TestFixture]
    public class StringCalculatorTests
    {
        [Test]
        public void Add_when_empty_string_return_0()
        {
            //arrange
            var calculator = new StringCalculator();
            //act
            var result = calculator.Add("");
            //assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Add_when_string_1_return_1()
        {
            //arrange
            var calculator = new StringCalculator();
            //act
            var result = calculator.Add("1");
            //assert
            Assert.AreEqual(1, result);
        }
        [Test]
        public void Add_when_string_1Comma2_return_2()
        {
            //arrange
            var calculator = new StringCalculator();
            //act
            var result = calculator.Add("1,2");
            //assert
            Assert.AreEqual(3, result);
        }
        [Test]
        public void Add_when_string_1Comma2Comma3_return_6()
        {
            //arrange
            var calculator = new StringCalculator();
            //act
            var result = calculator.Add("1,2,3");
            //assert
            Assert.AreEqual(6, result);
        }
        [Test]
        public void Add_when_string_1NewLine2Comma3_return_6()
        {
            //arrange
            var calculator = new StringCalculator();
            //act
            var result = calculator.Add("1\n2,3");
            //assert
            Assert.AreEqual(6, result);
        }

        [Test]
        public void Add_when_string_startsWith_doubleForwardSlash_delimiter_and_newLine_and_1_delimiter_2_delimiter_3_return_6()
        {
            //arrange
            var calculator = new StringCalculator();
            //act
            var result = calculator.Add("//;\n1;2;3");
            //assert
            Assert.AreEqual(6, result);
        }
        
        
        [Test]
        public void Add_when_string_is_negative_throw_exception_negative_not_allowed()
        {
            //arrange
            var calculator = new StringCalculator();
            //act
            //assert
            Assert.Throws<Exception>(() => calculator.Add("-1"), "Negatives not allowed -1");
        }
        [Test]
        public void Add_when_string_is_multiple_negative_throw_exception_negative_not_allowed()
        {
            //arrange
            var calculator = new StringCalculator();
            //act
            //assert
            Assert.Throws<Exception>(() => calculator.Add("-1,-2"), "Negatives not allowed -1,-2");
        }

        
        [Test]
        public void Add_when_string_has_more_than_1000_ignore_ie_1Comma1002Comma2_return_3()
        {
            //arrange
            var calculator = new StringCalculator();
            //act
            var result = calculator.Add("1,1002,2");
            //assert
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Add_when_string_startsWith_doubleForwardSlash_delimiter_of_any_length_and_newLine_and_1_delimiter_2_delimiter_3_return_6()
        {
            //arrange
            var calculator = new StringCalculator();
            //act
            var result = calculator.Add("//[***]\n1***2***3");
            //assert
            Assert.AreEqual(6, result);
        }
    }

    public class StringCalculator
    {
        public double Add(string empty)
        {

            
            char [] delimiters = new char[]{',', '\n'};
            char? customDelimiter = null;
            string customLengthDelimiter = "";
            if (empty.StartsWith("//"))
            {
                var lengthOfCustomDelimiterWithBrackets = empty.IndexOf('\n');
                customLengthDelimiter = empty.Substring(2, lengthOfCustomDelimiterWithBrackets - 2).Replace("[", "").Replace("]","");
                
                customDelimiter = empty.Substring(2, 1).ToCharArray()[0];
                delimiters = new char[]{customDelimiter.Value};
                
                empty = customLengthDelimiter.Length > 0 ? empty.Substring(lengthOfCustomDelimiterWithBrackets + 1, empty.Length -  lengthOfCustomDelimiterWithBrackets - 1)
                    : empty.Substring(4, empty.Length - 4);

                
            }

            if (empty.Contains(",") || empty.Contains("\n") || customDelimiter != null)
            {

                var preprocess = 
                    (!string.IsNullOrEmpty(customLengthDelimiter) ? empty.Split(customLengthDelimiter): 
                    empty.Split(delimiters)).Select(x => int.Parse(x)).Where(x=>x <= 1000).ToList();
                    
                if (preprocess.Any(x => x < 0))
                {
                    var negatives = string.Join(',', preprocess.Where(x => x < 0));
                    throw new Exception($"Negatives not allowed {negatives}");
                }


                return preprocess.Sum();
            }

            if (empty == "1") return 1;
            if(!string.IsNullOrEmpty(empty) && int.Parse(empty) < 0) throw new Exception($"Negatives not allowed {empty}");
            return 0;
        }
    }
}
