using Bumblebee;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace bumblebee
{
    class Program
    {
        /// <summary>
        /// Command-line C# refactoring tool.
        /// </summary>
        /// <param name="from">Expression to be replaced.</param>
        /// <param name="to">Expression to replace <paramref name="from"/> with.</param>
        /// <param name="solutionPath">Path to a solution; replacement occurs for all compilation units in the solution.</param>
        /// <returns></returns>
        static async Task Main(string from, string to, string? solutionPath = null)
        {
            MSBuildLocator.RegisterDefaults();

            var fromSnippet = new Snippet(from);
            var toSnippet = new Snippet(to);

            var workspace = MSBuildWorkspace.Create();
            var solution = await workspace.OpenSolutionAsync(solutionPath);

            foreach (var project in solution.Projects)
            {
                var compilation = await project.GetCompilationAsync();

                foreach (var syntaxTree in compilation.SyntaxTrees)
                {
                    var match = TreeMatcher.Match(syntaxTree.GetRoot(), fromSnippet);

                    if (match is object)
                    {
                        Console.WriteLine(match);
                    }
                }
            }
        }
    }
}
