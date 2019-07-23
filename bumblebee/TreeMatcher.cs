using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bumblebee
{
    public static class TreeMatcher
    {
        public static SyntaxNode? Match(SyntaxNode tree, Snippet snippet)
        {
            return Match(tree, snippet.Expression);
        }

        public static SyntaxNode? Match(SyntaxNode tree, SyntaxNode snippet)
        {
            SyntaxKind snippetKind = snippet.Kind();
            System.Console.WriteLine($"Trying to match a {snippetKind}");

            var snippetChildren = snippet.ChildNodes().ToArray();

            foreach (var descendant in tree.DescendantNodesAndSelf())
            {
                if (descendant.IsKind(snippetKind))
                {
                    // System.Console.WriteLine($"Match found: {descendant} is a {snippetKind}");

                    var treeNodes = descendant.DescendantNodes().ToArray();

                    for (int i = 0; i < snippetChildren.Length; i++)
                    {
                        SyntaxNode snippetNode = snippetChildren[i];
                        System.Console.WriteLine($"Descendant: {snippetNode}, a {snippetNode.Kind()}");
                        if (!LeafNodeMatches(treeNodes[i], snippetNode))
                        {
                            System.Console.WriteLine($"Leaf match failure");
                            break;
                        }
                    }

                    System.Console.WriteLine($"Calling it a match on {descendant} to {snippet}");

                    return descendant;
                }

                // System.Console.WriteLine($"Discarding {descendant}");
            }

            return null;
        }

        private static bool LeafNodeMatches(SyntaxNode left, SyntaxNode right)
        {
            if (!left.IsKind(right.Kind()))
            {
                return false;
            }

            switch ((left, right))
            {
                case (IdentifierNameSyntax leftIdentifier, IdentifierNameSyntax rightIdentifier):
                    System.Console.WriteLine($"Identifier {leftIdentifier} =? {rightIdentifier}");
                    if (leftIdentifier.Identifier.IsEquivalentTo(rightIdentifier.Identifier))
                    {
                        System.Console.WriteLine("*** match");
                        return true;
                    }

                    break;
            }

            return false;
        }
    }
}