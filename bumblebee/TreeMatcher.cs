using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bumblebee
{
    public static class TreeMatcher
    {
        private static readonly Regex SingleLowercaseCharacter = new Regex(@"^\p{Ll}$", RegexOptions.Compiled);
        

        public static SyntaxNode? Match(SyntaxNode tree, Snippet snippet)
        {
            return Match(tree, snippet.Expression);
        }

        public static SyntaxNode? Match(SyntaxNode haystack, SyntaxNode needle)
        {
            SyntaxKind needleRootKind = needle.Kind();
            //System.Console.WriteLine($"Trying to match a {needleRootKind}");

            var needleChildren = needle.ChildNodes().ToArray();

            // walk the full tree, 
            foreach (var descendant in haystack.DescendantNodesAndSelf())
            {
                // checking each node against the root of the needle
                if (!descendant.IsKind(needleRootKind))
                {
                    // This node doesn't match the root of the needle, so discard it.
                    // Since we're walking all descendant nodes, we'll find any children/
                    // grandchildren later.
                    continue;
                }

                // If the kind of the current node matches the root of the needle,
                // start checking its children

                var haystackChildren = descendant.ChildNodes().ToArray();

                // If there are different numbers of immediate children, fail
                if (haystackChildren.Length != needleChildren.Length)
                {
                    continue;
                }

                bool allChildrenMatch = true;

                for (int i = 0; i < needleChildren.Length; i++)
                {
                    //System.Console.WriteLine($"Descendant: {needleChildren[i]}, a {needleChildren[i].Kind()}");
                    if (!RootedRecursiveMatchWithWildcards(haystackChildren[i], needleChildren[i]))
                    {
                        //System.Console.WriteLine($"Recursive match failure");
                        allChildrenMatch = false;
                        break;
                    }
                }

                if (!allChildrenMatch)
                {
                    continue;
                }

                //System.Console.WriteLine($"Calling it a match on {descendant} to {needle}");

                return descendant;

                // System.Console.WriteLine($"Discarding {descendant}");
            }

            return null;
        }

        private static bool RootedRecursiveMatchWithWildcards(SyntaxNode haystack, SyntaxNode needle)
        {
            if (!haystack.IsKind(needle.Kind()))
            {
                if (IsWildcardExpression(needle) &&
                    haystack is ExpressionSyntax)
                {
                    // The current root of the needle is a wildcard, and the current
                    // root of the haystack is some kind of expression.  That's a match!
                    return true;
                }

                return false;
            }

            var haystackChildren = haystack.ChildNodes().ToArray();
            var needleChildren = needle.ChildNodes().ToArray();

            if (needleChildren.Length == 0)
            {
                // No children so this must be a primitive.

                switch (haystack, needle)
                {
                    case (IdentifierNameSyntax haystackName, IdentifierNameSyntax needleName):
                        return haystackName.Identifier.ValueText.Equals(needleName.Identifier.ValueText, StringComparison.Ordinal);
                    case (LiteralExpressionSyntax haystackLiteral, LiteralExpressionSyntax needleLiteral):
                        return haystackLiteral.Token.ValueText.Equals(needleLiteral.Token.ValueText, StringComparison.Ordinal);
                    default:
                        throw new NotImplementedException($"Don't understand primitive type comparison: {haystack} is {haystack.Kind()}, {needle} is {needle.Kind()}");
                }
            }

            // some children--the same number?
            if (haystackChildren.Length != needleChildren.Length)
            {
                return false;
            }

            // then recursively match on each
            for (int i = 0; i < needleChildren.Length; i++)
            {
                if (!RootedRecursiveMatchWithWildcards(haystackChildren[i], needleChildren[i]))
                {
                    // a child doesn't match, so fail out
                    return false;
                }
            }

            return true;
        }

        private static bool IsWildcardExpression(SyntaxNode n) =>
            n.IsKind(SyntaxKind.IdentifierName) &&
            SingleLowercaseCharacter.IsMatch(((IdentifierNameSyntax)n).Identifier.ValueText);
    }
}