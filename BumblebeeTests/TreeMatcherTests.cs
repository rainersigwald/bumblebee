using Bumblebee;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Xunit;
using Xunit.Abstractions;

namespace BumblebeeTests
{
    public class TreeMatcherTests
    {
        private readonly ITestOutputHelper output;

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


        public TreeMatcherTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void PerfectExpressionMatchCanMatchOnlyOneExpressionOfKind()
        {
            var match = TreeMatcher.Match(tree.GetRoot(), new Snippet("Console.WriteLine(\"Hello, World!\")"));

            match.Should().NotBeNull();
        }

        [Fact]
        public void StringLiteralDifferenceIsDetected()
        {
            var match = TreeMatcher.Match(tree.GetRoot(), new Snippet("Console.WriteLine(\"Something else\")"));

            match.Should().BeNull();
        }

        [Fact]
        public void DifferentLiteralTypeDifferenceIsDetected()
        {
            var match = TreeMatcher.Match(tree.GetRoot(), new Snippet("Console.WriteLine(12)"));

            match.Should().BeNull();
        }

        [Fact]
        public void WildcardMatchesStringLiteral()
        {
            var match = TreeMatcher.Match(tree.GetRoot(), new Snippet("Console.WriteLine(a)"));

            match.Should().NotBeNull();
        }

        [Theory]
        [InlineData("Foo.Baz()")]
        [InlineData("Baz.Bar()")]
        public void NoWildcardsExpressionAlmostMatches(string searchString)
        {
            var tree = SyntaxFactory.ParseExpression("Foo.Bar()");

            TreeMatcher.Match(tree, new Snippet(searchString))
                .Should().BeNull();
        }
    }
}
