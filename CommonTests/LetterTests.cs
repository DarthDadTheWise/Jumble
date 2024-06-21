namespace Common.Tests;

[TestClass()]
public class LetterTests
{
    [TestMethod()]
    public void EncodedTest()
    {
        Letter letter = new('A');
        Assert.AreEqual('A', letter.Encoded);

        letter = new('a');
        Assert.AreEqual('A', letter.Encoded);

        letter = new('-');
        Assert.AreEqual('-', letter.Encoded);

        letter = new(' ');
        Assert.AreEqual(' ', letter.Encoded);

        letter = new('_');
        Assert.AreEqual('_', letter.Encoded);

        letter = new('?');
        Assert.AreEqual('?', letter.Encoded);
    }

    [TestMethod()]
    public void DecodedTest()
    {
        Letter letter = new('A');
        Assert.AreEqual('_', letter.Decoded);

        letter.Decoded = 'b';
        Assert.AreEqual('B', letter.Decoded);

        Letter letterC = new('c')
        {
            Decoded = 'C'
        };
        Assert.AreEqual('_', letterC.Decoded);

        letter = new('-');
        Assert.AreEqual('-', letter.Decoded);
    }

    [TestMethod()]
    public void EncodedSymbolTest()
    {
        Letter letter = new('-');
        Assert.AreEqual('-', letter.Decoded);

        letter.Decoded = '?';
        Assert.AreEqual('?', letter.Decoded);
    }

    [TestMethod()]
    public void IsSolvedTest()
    {
        Letter letter = new('A');
        Assert.IsFalse(letter.IsSolved);

        letter.Decoded = 'b';
        Assert.IsTrue(letter.IsSolved);

        letter.Decoded = ' ';
        Assert.IsTrue(letter.IsSolved);

        letter = new('_');
        Assert.IsFalse(letter.IsSolved);
    }
}
