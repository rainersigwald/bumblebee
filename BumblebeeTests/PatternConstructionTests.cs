using Bumblebee;
using FluentAssertions;
using Xunit;

namespace BumblebeeTests
{
    public class PatternConstructionTests
    {
        [Theory]
        [InlineData("a.M()")]
        [InlineData("a.M")]
        [InlineData("a + 1")]
        [InlineData("a()")]
        [InlineData("(a)")]
        [InlineData("Method(a)")]
        [InlineData("MethodWithALowercaseA(a)")]
        [InlineData(@"a = ""a""")]
        [InlineData(@"a + a + a")]
        [InlineData(@"[a]")]
        [InlineData(@"Something[a]")]
        public void SinglePatternMatch(string snippet)
        {
            var s = new Snippet(snippet);

            s.SubexpressionIdentifiers.Should().HaveCount(1).And.Contain("a");
        }

        [Theory]
        [InlineData("a + b")]
        [InlineData("a.b")]
        [InlineData("a.Equals(b)")]
        [InlineData("b.Equals(a)")]
        [InlineData("[a(b)]")]
        [InlineData("a[b]")]
        public void TwoPatternMatches(string snippet)
        {
            var s = new Snippet(snippet);

            s.SubexpressionIdentifiers.Should().HaveCount(2).And.Contain("a").And.Contain("b");
        }
    }
}