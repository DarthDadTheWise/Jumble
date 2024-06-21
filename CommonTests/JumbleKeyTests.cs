namespace Common.Tests;


[TestClass()]
public class JumbleKeyTests
{
    [TestMethod()]
    public void ToKeyStringTest()
    {
        JumbleKey key = new();
        var expected = "__________________________";
        var actual = key.KeyString;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void GetLetterTest()
    {
        JumbleKey key = new();
        Assert.AreEqual('A', key['A'].Encoded);
        Assert.AreEqual('A', key['a'].Encoded);
        Assert.AreEqual(' ', key[' '].Encoded);
        Assert.AreEqual('_', key['_'].Encoded);
        Assert.AreEqual('?', key['?'].Encoded);
    }

    [TestMethod()]
    public void GetLettersTest()
    {
        JumbleKey key = new();
        var actual = key["hello"];
        Assert.AreEqual(5, actual.Count);
        Assert.AreEqual('H', actual[0].Encoded);
        Assert.AreEqual('E', actual[1].Encoded);
        Assert.AreEqual('L', actual[2].Encoded);
        Assert.AreEqual('L', actual[3].Encoded);
        Assert.AreEqual('O', actual[4].Encoded);

        actual = key["we've"];
        Assert.AreEqual(5, actual.Count);
        Assert.AreEqual('W', actual[0].Encoded);
        Assert.AreEqual('E', actual[1].Encoded);
        Assert.AreEqual('\'', actual[2].Encoded);
        Assert.AreEqual('V', actual[3].Encoded);
        Assert.AreEqual('E', actual[4].Encoded);

        actual = key["ten-dollars"];
        Assert.AreEqual(11, actual.Count);
        Assert.AreEqual('N', actual[2].Encoded);
        Assert.AreEqual('-', actual[3].Encoded);
        Assert.AreEqual('D', actual[4].Encoded);

        actual = key["one two"];
        Assert.AreEqual(7, actual.Count);
        Assert.AreEqual('E', actual[2].Encoded);
        Assert.AreEqual(' ', actual[3].Encoded);
        Assert.AreEqual('T', actual[4].Encoded);
    }

    [TestMethod()]
    public void UniqueLetterTest()
    {
        JumbleKey key = new();
        Assert.AreEqual("__________________________", key.KeyString);
        Word word = new(key, "ABCCD");
        Assert.AreEqual("__________________________", key.KeyString);
        word.Decoded = "HELLO";
        Assert.AreEqual("HELO______________________", key.KeyString);
        key['A'].Decoded = 'H';
        Assert.AreEqual("HELO______________________", key.KeyString);
        key['B'].Decoded = 'H';
        Assert.AreEqual("_HLO______________________", key.KeyString);
        key['C'].Decoded = 'H';
        Assert.AreEqual("__HO______________________", key.KeyString);
        key['D'].Decoded = 'H';
        Assert.AreEqual("___H______________________", key.KeyString);
        key['E'].Decoded = 'H';
        Assert.AreEqual("____H_____________________", key.KeyString);
    }

    [TestMethod()]
    public void IsLetterUsedTest()
    {
        JumbleKey key = new();
        Assert.IsFalse(key.IsLetterUsed('Z'));
        Assert.IsFalse(key.IsLetterUsed(' '));
        Assert.IsFalse(key.IsLetterUsed('_'));
        Assert.IsFalse(key.IsLetterUsed('-'));
        Assert.IsFalse(key.IsLetterUsed('?'));
        Assert.IsFalse(key.IsLetterUsed('.'));

        key['A'].Decoded = 'Z';
        Assert.IsTrue(key.IsLetterUsed('Z'));
    }

    [TestMethod()]
    public void IsLetterUsedTest1()
    {
        JumbleKey key = new();
        Word word = new(key, "ABC");
        key.KeyString = "T_EU______________________";

        Assert.IsTrue(key.IsLetterUsed('T'));
        Assert.IsFalse(key.IsLetterUsed('H'));
        Assert.IsTrue(key.IsLetterUsed('E'));
        Assert.IsTrue(key.IsLetterUsed('U'));
    }
}