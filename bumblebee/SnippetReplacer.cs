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
            return original.ReplaceNodes(TreeMatcher.Matches(original, from),
                (m, n) => SyntaxFactory.ParseName("replacement"));
        }
    }
}