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

            await RunAsync(solutionPath, from, to);
        }

        private static async Task RunAsync(string? solutionPath, string from, string to)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = await workspace.OpenSolutionAsync(solutionPath);

            var p = solution.Projects.First();

            var c = await p.GetCompilationAsync();

            //var s = c.SyntaxTrees.First();

            //var r = await s.GetRootAsync();
        }
    }
}
