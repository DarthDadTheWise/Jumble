using Common;
using System.Diagnostics;

namespace JumbleSolver;

class Program
{
    static void Main(string[] _)
    {
        var stopwatch = new Stopwatch();
        try
        {
            stopwatch.Start();

            //Puzzle puzzle;
            //puzzle = new("A LIE CAN RUN ROUND THE WORLD BEFORE THE TRUTH HAS GOT ITS BOOTS ON.");
            //puzzle.Key.KeyString = "BCDEFGHIJKLMNOPQRSTUVWXYZA";
            //Console.WriteLine(puzzle.Decoded);

            Solve("B MJF DBO SVO SPVOE UIF XPSME CFGPSF UIF USVUI IBT HPU JUT CPPUT PO.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            stopwatch.Stop();
            Console.WriteLine($"Completed in {stopwatch.ElapsedMilliseconds} milliseconds.\n");
            Console.WriteLine("Press Enter to quit");
            Console.ReadLine();
        }
    }

    static public void Solve(string encoded)
    {
        Puzzle puzzle = new(encoded);
        puzzle.Solve();
        Console.WriteLine(puzzle.IsSolved ? "Solved" : "Unsolved");
        Console.WriteLine(puzzle.Encoded);
        Console.WriteLine(puzzle.Decoded);
        Console.WriteLine("");
    }
}
