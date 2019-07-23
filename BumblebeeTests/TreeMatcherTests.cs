using Bumblebee;
using FluentAssertions;
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

        public TreeMatcherTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void PerfectExpressionMatchCanMatchOnlyOneExpressionOfKind()
        {
var tree = CSharpSyntaxTree.ParseText(
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
            var match = TreeMatcher.Match(tree.GetRoot(), new Snippet("Console.WriteLine(\"Hello, World\")"));

            match.Should().NotBeNull();
        }

        [Fact]
        public void NoWildcardsExpressionAlmostMatches()
        {
            var tree = SyntaxFactory.ParseExpression("Foo.Bar()");

            TreeMatcher.Match(tree, new Snippet("Foo.Baz()"))
                .Should().BeNull();

        }
    }
}
