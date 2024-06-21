using CommonTests;

namespace Common.Tests;

[TestClass()]
public class PuzzleTests
{
    [TestMethod()]
    public void EncodeTest()
    {
        Puzzle puzzle;
        foreach (var testcase in TestData.Puzzles)
        {
            puzzle = new($" {testcase.Key.ToLower()} ");
            Assert.AreEqual(testcase.Key, puzzle.Encoded);
        }
    }

    [TestMethod()]
    public void DecodeTest()
    {
        Puzzle puzzle;
        var decodeKey = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
        foreach (var testcase in TestData.Puzzles)
        {
            puzzle = new(testcase.Key);
            puzzle.Key.KeyString = decodeKey;
            Assert.AreEqual(testcase.Value, puzzle.Decoded);
        }
    }

    [TestMethod()]
    public void DecodeDefaultTest()
    {
        Puzzle puzzle;
        foreach (var testcase in TestData.Placeholders)
        {
            puzzle = new(testcase.Key);
            Assert.AreEqual(testcase.Value, puzzle.Decoded);
        }
    }

    [TestMethod()]
    public void SolveTest()
    {
        Puzzle puzzle;
        foreach (var testcase in TestData.Puzzles)
        {
            puzzle = new(testcase.Key);
            puzzle.Solve();
            Assert.AreEqual(testcase.Value, puzzle.Decoded);
        }
    }

    [TestMethod()]
    public void IsSolvedTest()
    {
        Puzzle puzzle;
        foreach (var testcase in TestData.Puzzles)
        {
            puzzle = new(testcase.Key);
            puzzle.Solve();
            Assert.IsTrue(puzzle.IsSolved);
        }
    }
}