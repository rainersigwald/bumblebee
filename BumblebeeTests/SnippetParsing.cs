using Bumblebee;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Xunit;

namespace BumblebeeTests
{
    public class SnippetParsing
    {
        [Fact]
        public static void BareIdentifierNameThrows()
        {
            Assert.Throws<UnknownExpressionException>(() =>
                    new Snippet("a"));
        }

        [Fact]
        public static void IdentifierDotMethod()
        {
            var s = new Snippet("");

            s.Expression.Kind().Should().Be(SyntaxKind.InvocationExpression);

            var i = (InvocationExpressionSyntax)s.Expression;
            i.ArgumentList.Arguments.Should().BeEmpty();
            i.Expression.Kind().Should().Be(SyntaxKind.SimpleMemberAccessExpression);

            var smae = (MemberAccessExpressionSyntax)i.Expression;
            smae.Name.Identifier.ValueText.Should().Be("M");

            var identifierExpression = (IdentifierNameSyntax)smae.Expression;
            identifierExpression.Identifier.ValueText.Should().Be("a");
        }
    }
}
