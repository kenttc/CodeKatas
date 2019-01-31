using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CodeKatasTests
{
    [TestFixture]
    public class SimpleCalculatorTests
    {
        [Test]
        public void When_Add_Empty_string_returns_zero()
        {
            //arrange
            var service = new SimpleCalculator();
            //act
            var result = service.Add("");
            //assert
            Assert.AreEqual(0, result);
        }
        [Test]
        public void When_Add_1_string_returns_1()
        {
            //arrange
            var service = new SimpleCalculator();
            //act
            var result = service.Add("1");
            //assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void When_Add_1Comma2_string_returns_3()
        {
            //arrange
            var service = new SimpleCalculator();
            //act
            var result = service.Add("1,2");
            //assert
            Assert.AreEqual(3, result);
        }

        [Test]
        public void When_Add_1Comma2Comma3_string_returns_6()
        {
            //arrange
            var service = new SimpleCalculator();
            //act
            var result = service.Add("1,2,3");
            //assert
            Assert.AreEqual(6, result);
        }

        [Test]
        public void When_Add_1newLine2Comma3_string_returns_6()
        {
            //arrange
            var service = new SimpleCalculator();
            //act
            var result = service.Add("1\n2,3");
            //assert
            Assert.AreEqual(6, result);
        }
        [Test]
        public void When_Add_consecutive_delimiters_string_throws_error()
        {
            //arrange
            var service = new SimpleCalculator();
            //act//assert
            Assert.Throws<ArgumentException>(()=>service.Add("1\n,3"));
            
            
        }
        [Test]
        public void When_Add_has_delimiter_defined_in_the_first_line_of_string_will_use_delimiter_as_provided()
        {
            //arrange
            var service = new SimpleCalculator();
            //act
            var result = service.Add("//;\n1;2;3");
            
            //assert
            Assert.AreEqual(6, result);
            
            
        }

        [Test]
        public void When_Add_has_negatives_throw_exception_with_message_negatives_not_allowed()
        {
            //arrange
            var service = new SimpleCalculator();
            //act//assert
            
            Assert.Throws<Exception>(()=>service.Add("-1,-3"), "negatives not allowed -1, -3"); 
            
        }
        [Test]
        public void When_Add_has_numbers_more_than_1000_ignore_numbers_more_than_1000_from_summation()
        {
            //arrange
            var service = new SimpleCalculator();
            //act
            var result = service.Add("1001,2");
            
            //assert
            Assert.AreEqual(2, result);
            
        }

        [Test]
        public void When_Add_has_delimiters_of_any_length_can_still_use_provided_delimiters_to_sum()
        {
            //arrange
            var service = new SimpleCalculator();
            //act
            var result = service.Add("//[***]\n1***2***3");
            
            //assert
            Assert.AreEqual(6, result);
            
        }

        [Test]
        public void When_Add_has_multiple_delimiters_of_any_length_can_still_use_provided_delimiters_to_sum()
        {
            //arrange
            var service = new SimpleCalculator();
            //act
            var result = service.Add("//[**][%]\n1**2%3");
            
            //assert
            Assert.AreEqual(6, result);
            
        }
    }

    public class SimpleCalculator
    {
        public object Add(string empty)
        {

            if (string.IsNullOrEmpty(empty)) return 0;
            var charArrayDelimiter = new char[]{',', '\n'};
            
            string[] numbers = empty.Split(charArrayDelimiter);

            if (empty.StartsWith("//"))
            {
                var newDelimiterLength = empty.IndexOf('\n') - 2;
                
                var newDelimiters = empty.Substring(2, newDelimiterLength);

                var stringArrayDelimiter = new string[] { };
                var customLengthDelimitersCount = empty.ToCharArray().Count(x => x == ']');
            
                if (customLengthDelimitersCount > 1)
                {
                     stringArrayDelimiter = newDelimiters
                        .Substring(1, newDelimiters.Length - 2)
                        .Split(new string[]{"]["}, StringSplitOptions.None);

                }
                else if(customLengthDelimitersCount == 1)
                {
                    var newDelimiter = empty.Substring(2, newDelimiterLength).Replace("[", "").Replace("]", "");
                    stringArrayDelimiter = new string[]{newDelimiter};
                }
                else
                {
                    charArrayDelimiter = newDelimiters.ToCharArray();
                }

                

                empty = empty.Substring(3+ newDelimiterLength, empty.Length - 3 - newDelimiterLength);

                numbers = newDelimiterLength > 1 ? 
                    empty.Split(stringArrayDelimiter, StringSplitOptions.None) 
                    : empty.Split(charArrayDelimiter);
            }

             
            
            if(numbers.Any(x=>string.IsNullOrEmpty(x)))
                throw new ArgumentException();

            var convertedNumbers = numbers.Select(x => int.Parse(x)).ToList();
            if (convertedNumbers.Any(x => x < 0))
            {
                var negatives = string.Join(',',convertedNumbers.Where(x => x < 0).ToList());
                throw new Exception($"negatives not allowed {negatives}");
            }

            return convertedNumbers.Where(x=> x <= 1000).Sum();
            
            
        }
    }
}
