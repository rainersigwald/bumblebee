using Bumblebee;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace BumblebeeTests
{
    public class ReplaceSnippetTests
    {
        [Fact]
        public void ReplaceExpressionWithConstant()
        {
            tree.GetRoot()
                .ReplaceSnippet(new Snippet(@"Console.WriteLine(""Hello, World!"")"),
                    new Snippet("variableReference"))
                .ToFullString()
                .Should().NotContainAny("Console.WriteLine", "Hello, World!");
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