using NUnit.Framework;
using System;
using System.Runtime.InteropServices;
/*
 * 
 *  Read: https://docs.microsoft.com/en-us/archive/msdn-magazine/2018/january/csharp-all-about-span-exploring-a-new-net-mainstay
 * 
 */
namespace Spans
{
    class Program
    {
        static void Main(string[] args)
        {
            Program prog = new Program();

            prog.SpanWithStackAlloc();
            prog.ReadOnlySpanFromString();
            prog.ReadOnlySpanParsing();
        }       

        /// <summary>
        ///     Using ReadOnlySpan to slice a string and parse the slice into a integer<br/>
        ///     This is good when you want to parse strings or parts of strings and you don't want to modify the integer when it is in string form       
        /// </summary>
        void ReadOnlySpanParsing()
        {
            string srcStr = "3489,3972";

            // Creating a stack alloc srcSpan to ref our srcStr on the heap
            ReadOnlySpan<char> srcSpan = srcStr;

            // Parsing the sliced srcStr, no allocation on the heap here
            // Substring would perform a heap allocation and thereby decrease perform
            int numOne = int.Parse(srcSpan.Slice(0, 4));
            int numTwo = int.Parse(srcSpan.Slice(5, 4));

            Assert.AreEqual(3489, numOne);
            Assert.AreEqual(3972, numTwo);
        }

        /// <summary>
        ///     Using ReadOnlySpan to work with a string without allocating new strings.<br/>
        ///     Also using the CompareTo function to do comparisions against ReadOnlySpans<br/>
        ///     This is good when you want to use a substring function but dont plan on editing the src
        /// </summary>
        void ReadOnlySpanFromString()
        {
            string strSrc = "Hello World";  // Source String <-- Alloc onto heap
            // strSrc.Substring(0, 5);  <-- Allocation onto heap
            ReadOnlySpan<char> helloSpan = strSrc.AsSpan().Slice(0, 5); // <-- Stack alloc
            ReadOnlySpan<char> worldSpan = strSrc.AsSpan().Slice(6, 5); // <-- Stack alloc

            Assert.AreEqual('H', helloSpan[0]);
            Assert.AreEqual('e', helloSpan[1]);
            Assert.AreEqual('l', helloSpan[2]);
            Assert.AreEqual('l', helloSpan[3]);
            Assert.AreEqual('o', helloSpan[4]);

            Assert.AreEqual('W', worldSpan[0]);
            Assert.AreEqual('o', worldSpan[1]);
            Assert.AreEqual('r', worldSpan[2]);
            Assert.AreEqual('l', worldSpan[3]);
            Assert.AreEqual('d', worldSpan[4]);

            Assert.AreEqual(-15, helloSpan.CompareTo(worldSpan, StringComparison.Ordinal));
            Assert.AreEqual(15, worldSpan.CompareTo(helloSpan, StringComparison.Ordinal));

            int result = helloSpan.CompareTo(strSrc, StringComparison.Ordinal);
            if (15 == result)
            {
                // sender span is after other
            }
            else if (-15 == result)
            {
                // sender span is before other
            }
            else
            {
                // equal
            }
        }

        /// <summary>
        ///     Using a span to work with a stack allocated array of integers<br/>
        ///     This would be good when you have a temporary array you only want within your method's scope
        /// </summary>
        void SpanWithStackAlloc()
        {
            // Allocating a 
            Span<byte> bytes = stackalloc byte[2];            
            bytes[0] = 20;
            bytes[1] = byte.MaxValue;

            // Testing
            Assert.AreEqual(20, bytes[0]);
            Assert.AreEqual(Byte.MaxValue, bytes[1]);
        }
    }    
}
