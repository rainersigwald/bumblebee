using Bumblebee;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace BumblebeeTests
{
    public class ReplaceSnippetTests
    {
        [Theory]
        [InlineData(@"Console.WriteLine(""Hello, World!"")")]
        [InlineData(@"Console.WriteLine(a)")]
        [InlineData(@"Console.a(b)")]
        [InlineData(@"a(""Hello, World!"")")]
        public void ReplaceExpressionWithConstant(string fromText)
        {
            tree.GetRoot()
                .ReplaceSnippet(new Snippet(fromText),
                    new Snippet("variableReference"))
                .ToFullString()
                .Should().NotContainAny("Console.WriteLine", "Hello, World!")
                .And.Contain("variableReference")
                .And.NotMatchRegex("\nvariableReference", because: "The leading trivia should be preserved");
        }

        private readonly SyntaxTree tree = CSharpSyntaxTree.ParseText(
@"using System;
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}");

    }
}