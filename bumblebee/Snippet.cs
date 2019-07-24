using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bumblebee
{
    /// <summary>
    /// Code blob, possibly containing wildcard matches.
    /// </summary>
    public class Snippet
    {
        /// <summary>
        /// The <see cref="ExpressionSyntax" /> that includes the wildcards as identifiers.
        /// </summary>
        public readonly ExpressionSyntax Expression;

        /// <summary>
        /// Construct a new <see cref="Snippet"/> from a string containing a C# expression.
        /// </summary>
        /// <param name="text">String containing a C# expression, possibly with wildcards (single-character variables).</param>
        public Snippet(string? text)
        {
            // TODO: try parsing as statement first?
            Expression = SyntaxFactory.ParseExpression(text);

            // TODO: make this error! just not sure . . . how.

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

        /// <summary>
        /// List of identifiers found in the expression that will be treated as wildcards.
        /// </summary>
        public List<string> SubexpressionIdentifiers { get; private set; }
    }
}