using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bumblebee
{
    public class Snippet
    {
        public readonly ExpressionSyntax Expression;

        public Snippet(string text)
        {
            Expression = SyntaxFactory.ParseExpression(text);

            if (!Expression.DescendantNodes().Any())
            {
                throw new UnknownExpressionException();
            }

            var lc = new Regex(@"^\p{Ll}$");

            var replacementNodes = Expression.DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .Where(nameSyntax => lc.IsMatch(nameSyntax.Identifier.ValueText))
                .GroupBy(nameSyntax => nameSyntax.Identifier.ValueText);

            SubexpressionIdentifiers = replacementNodes
                .Select(g => g.Key)
                .ToList();
        }

        public List<string> SubexpressionIdentifiers { get; private set; }
    }
}