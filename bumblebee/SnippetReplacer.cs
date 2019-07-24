using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bumblebee
{
    /// <summary>
    /// Class containing extension methods to replace snippets in <see cref="SyntaxTree" />s.
    /// </summary>
    public static class SnippetReplacer
    {
        /// <summary>
        /// Replace all instances of <paramref name="from"/> transformed to <paramref name="to" />.
        /// </summary>
        public static SyntaxNode ReplaceSnippet(this SyntaxNode original, Snippet from, Snippet to)
        {
            return original.ReplaceNodes(TreeMatcher.Matches(original, from), GenerateReplacementNode(from, to));
        }

        private static Func<SyntaxNode, SyntaxNode, SyntaxNode> GenerateReplacementNode(Snippet from, Snippet to)
        {
            if (to.SubexpressionIdentifiers.Any())
            {
                throw new NotImplementedException("sorry, only smart enough to do constants in the 'to' snippet right now");
            }

            return (original, withModifiedChildren) =>
            {
                return to.Expression.WithLeadingTrivia(original.GetLeadingTrivia()).WithTrailingTrivia(original.GetTrailingTrivia());
            };
        }
    }
}