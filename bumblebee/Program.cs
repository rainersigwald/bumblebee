using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace bumblebee
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MSBuildLocator.RegisterDefaults();

            Console.WriteLine("Hello World!");

            await RunAsync();
        }

        private static async Task RunAsync()
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = await workspace.OpenSolutionAsync(@"S:\bumblebee\Bumblebee.sln");

            var p = solution.Projects.First();

            var c = await p.GetCompilationAsync();

            //var s = c.SyntaxTrees.First();

            //var r = await s.GetRootAsync();

            //foreach (Microsoft.CodeAnalysis.SyntaxNode n in r.DescendantNodes())
            //{
            //    Console.WriteLine($"{n.GetType()}: {n}");
            //}

            foreach (var syntaxTree in c.SyntaxTrees)
            {
                var rewriter = new Rewriter();

                var newSource = rewriter.Visit(syntaxTree.GetRoot());
            }
        }
    }
}
