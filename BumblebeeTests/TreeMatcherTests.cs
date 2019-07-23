using Bumblebee;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Xunit;

namespace BumblebeeTests
{
    public class TreeMatcherTests
    {
        [Fact]
        public static void PerfectExpressionMatchCanMatchOnlyOneExpressionOfKind()
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
            TreeMatcher.Match(tree.GetRoot(), new Snippet("Console.WriteLine(\"Hello, World!\")"))
            .Should().NotBeNull();
        }
    }
}
