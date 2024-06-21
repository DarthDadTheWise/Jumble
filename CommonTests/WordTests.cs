namespace Common.Tests;

[TestClass()]
public class WordTests
{
    [TestMethod()]
    public void EncodedTest()
    {
        JumbleKey key = new();
        var encoded = "";
        var expected = "";
        Word actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Encoded);

        encoded = "a";
        expected = "A";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Encoded);

        encoded = "A";
        expected = "A";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Encoded);

        encoded = "TEN";
        expected = "TEN";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Encoded);

        encoded = "TEN-DOLLAR";
        expected = "TEN-DOLLAR";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Encoded);
    }

    [TestMethod()]
    public void DecodedTest()
    {
        JumbleKey key = new();
        var encoded = "";
        var expected = "";
        Word actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Decoded);

        encoded = "a";
        expected = "_";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Decoded);

        encoded = "A";
        expected = "_";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Decoded);

        encoded = "TEN";
        expected = "___";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Decoded);

        encoded = "TEN-DOLLAR";
        expected = "___-______";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Decoded);
    }

    [TestMethod()]
    public void Decoded2Test()
    {
        JumbleKey key = new();
        Word word = new(key, "ABC");
        Assert.AreEqual("___", word.Decoded);
        Assert.AreEqual('_', word.Letters[0].Decoded);
        Assert.AreEqual('_', word.Letters[1].Decoded);
        Assert.AreEqual('_', word.Letters[2].Decoded);

        key.KeyString = "T_E_______________________";
        Assert.AreEqual("T_E", word.Decoded);
    }

    [TestMethod()]
    public void SetDecodedTest()
    {
        JumbleKey key = new();
        var encoded = "";
        var expected = "";
        Word actual = new(key, encoded);
        var previous = actual.Decoded;
        Assert.AreEqual(expected, actual.Decoded);
        actual.Decoded = previous;
        Assert.AreEqual(previous, actual.Decoded);

        key = new();
        encoded = "a";
        expected = "I";
        actual = new(key, encoded);
        previous = actual.Decoded;
        actual.Decoded = expected;
        Assert.AreEqual(expected, actual.Decoded);
        actual.Decoded = previous;
        Assert.AreEqual(previous, actual.Decoded);

        key = new();
        encoded = "AB";
        expected = "HI";
        actual = new(key, encoded);
        previous = actual.Decoded;
        actual.Decoded = expected;
        Assert.AreEqual(expected, actual.Decoded);
        actual.Decoded = previous;
        Assert.AreEqual(previous, actual.Decoded);

        key = new();
        encoded = "ABCCD";
        expected = "HELLO";
        actual = new(key, encoded);
        previous = actual.Decoded;
        actual.Decoded = expected;
        Assert.AreEqual(expected, actual.Decoded);
        actual.Decoded = previous;
        Assert.AreEqual(previous, actual.Decoded);

        key = new();
        encoded = "ABC-XEFFGH";
        expected = "TEN-DOLLAR";
        actual = new(key, encoded);
        previous = actual.Decoded;
        actual.Decoded = expected;
        Assert.AreEqual(expected, actual.Decoded);
        actual.Decoded = previous;
        Assert.AreEqual(previous, actual.Decoded);

        key = new();
        encoded = "A'B";
        expected = "I'M";
        actual = new(key, encoded);
        previous = actual.Decoded;
        actual.Decoded = expected;
        Assert.AreEqual(expected, actual.Decoded);
        actual.Decoded = previous;
        Assert.AreEqual(previous, actual.Decoded);

        key = new();
        encoded = "A'BB";
        expected = "I'LL";
        actual = new(key, encoded);
        previous = actual.Decoded;
        actual.Decoded = expected;
        Assert.AreEqual(expected, actual.Decoded);
        actual.Decoded = previous;
        Assert.AreEqual(previous, actual.Decoded);
    }

    [TestMethod()]
    public void LettersTest()
    {
        JumbleKey key = new();
        var encoded = "UJNFT";
        Word word = new(key, encoded);
        Assert.AreEqual(5, word.Letters.Count);
        Assert.AreEqual(encoded[0], word.Letters[0].Encoded);
        Assert.AreEqual(encoded[1], word.Letters[1].Encoded);
        Assert.AreEqual(encoded[2], word.Letters[2].Encoded);
        Assert.AreEqual(encoded[3], word.Letters[3].Encoded);
        Assert.AreEqual(encoded[4], word.Letters[4].Encoded);
    }

    [TestMethod()]
    public void PatternTest()
    {
        JumbleKey key = new();
        var encoded = "";
        var expected = "";
        Word actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Pattern);

        encoded = "a";
        expected = "A";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Pattern);

        encoded = "A";
        expected = "A";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Pattern);

        encoded = "TEN";
        expected = "ABC";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Pattern);

        encoded = "TEN-DOLLAR";
        expected = "ABC-DEFFGH";
        actual = new(key, encoded);
        Assert.AreEqual(expected, actual.Pattern);
    }

    [TestMethod()]
    public void PartsWithPatternTest()
    {
        JumbleKey key = new();
        Word word = new(key, "");
        var actual = word.PartsWithPattern;
        Assert.AreEqual(0, actual.Count);

        word = new(key, "Z");
        actual = word.PartsWithPattern;
        Assert.AreEqual(2, actual.Count);
        Assert.AreEqual("A", actual[0]);
        Assert.AreEqual("I", actual[1]);
    }

    [TestMethod()]
    public void IsSolvedTest()
    {
        JumbleKey key = new();
        Word word = new(key, "");
        Assert.IsFalse(word.IsSolved);

        word = new(key, "a");
        Assert.IsFalse(word.IsSolved);
        word.Letters[0].Decoded = 'B';
        Assert.IsTrue(word.IsSolved);

        word = new(key, "A");
        Assert.IsTrue(word.IsSolved);

        word = new(key, "TEN");
        Assert.IsFalse(word.IsSolved);

        word = new(key, "TEN-DOLLAR");
        Assert.IsFalse(word.IsSolved);
    }

    [TestMethod()]
    public void IsSolvedLettersTest()
    {
        JumbleKey key = new();
        key.KeyString = "T_E_______________________";
        Word word = new(key, "ABC");
        word.Decoded = "T H";

        word = new(key, "d");
        Assert.IsFalse(word.IsSolved);
        word.Letters[0].Decoded = 'B';
        Assert.IsTrue(word.IsSolved);

        word = new(key, "A");
        Assert.IsTrue(word.IsSolved);

        word = new(key, "TEN");
        Assert.IsFalse(word.IsSolved);

        word = new(key, "TEN-DOLLAR");
        Assert.IsFalse(word.IsSolved);
    }

    [TestMethod()]
    public void MakePatternLettersTest()
    {
        Assert.AreEqual("A", Word.MakePattern("I"));
        Assert.AreEqual("AB", Word.MakePattern("HI"));
        Assert.AreEqual("ABCCD", Word.MakePattern("HELLO"));
    }

    [TestMethod()]
    public void MakePatternSympolsTest()
    {
        Assert.AreEqual("AB'C", Word.MakePattern("IT'S"));
        Assert.AreEqual("A'BB", Word.MakePattern("I'LL"));
        Assert.AreEqual("AA-BAA", Word.MakePattern("MM-HMM"));
    }

    [TestMethod()]
    public void IsPossibleForWordEmptyTest()
    {
        JumbleKey key = new();
        Word word = new(key, "");
        Assert.IsTrue(word.IsPossible(""));
    }

    [TestMethod()]
    public void IsPossibleForWordABCTest()
    {
        JumbleKey key = new();
        Word word = new(key, "XYZ");
        Assert.IsFalse(word.IsPossible("___"));
        Assert.IsFalse(word.IsPossible("__C"));
        Assert.IsFalse(word.IsPossible("_B_"));
        Assert.IsFalse(word.IsPossible("_BC"));
        Assert.IsFalse(word.IsPossible("A__"));
        Assert.IsFalse(word.IsPossible("A_C"));
        Assert.IsFalse(word.IsPossible("AB_"));
        Assert.IsTrue(word.IsPossible("ABC"));

        Assert.IsFalse(word.IsPossible("   "));
        Assert.IsFalse(word.IsPossible("__Z"));
        Assert.IsFalse(word.IsPossible("_Y_"));
        Assert.IsFalse(word.IsPossible("_YZ"));
        Assert.IsFalse(word.IsPossible("X__"));
        Assert.IsFalse(word.IsPossible("X_Z"));
        Assert.IsFalse(word.IsPossible("XY_"));
        Assert.IsFalse(word.IsPossible("XYZ"));

        Assert.IsFalse(word.IsPossible("AA_"));
        //Assert.IsFalse(word.IsPossible("AAC"));
        Assert.IsFalse(word.IsPossible(""));
        Assert.IsFalse(word.IsPossible("A"));
        Assert.IsFalse(word.IsPossible("ABCD"));
    }

    [TestMethod()]
    public void IsPossibleForWordABBTest()
    {
        JumbleKey key = new();
        Word word = new(key, "XYY");
        Assert.IsFalse(word.IsPossible("___"));
        Assert.IsFalse(word.IsPossible("__C"));
        Assert.IsFalse(word.IsPossible("_B_"));
        Assert.IsFalse(word.IsPossible("_BB"));
        Assert.IsFalse(word.IsPossible("A__"));
        Assert.IsFalse(word.IsPossible("A_C"));
        Assert.IsFalse(word.IsPossible("AB_"));
        Assert.IsTrue(word.IsPossible("ABB"));

        Assert.IsFalse(word.IsPossible("   "));
        Assert.IsFalse(word.IsPossible("_BC"));
        //Assert.IsFalse(word.IsPossible("ABC"));
    }

    [TestMethod()]
    public void IsPossibleForWordListTest()
    {
        JumbleKey key = new();
        Word word = new(key, "Z");
        var actual = word.PartsWithPattern;
        Assert.AreEqual(2, actual.Count);
        Assert.AreEqual("A", actual[0]);
        Assert.AreEqual("I", actual[1]);

        word = new(key, "ABC");
        //key.KeyString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        key.KeyString = "T_E_______________________";

        // Filter out words that don't fit existing decoded letters
        List<string> partsWithDecoded = [];
        foreach (var part in word.PartsWithPattern)
        {
            if (word.IsPossible(part))
            {
                partsWithDecoded.Add(part);
            }
        }
        Assert.AreEqual(4, partsWithDecoded.Count);
        Assert.AreEqual("THE", partsWithDecoded[0]);
        Assert.AreEqual("TUE", partsWithDecoded[1]);
        Assert.AreEqual("TIE", partsWithDecoded[2]);
        Assert.AreEqual("TOE", partsWithDecoded[3]);

        key.KeyString = "T_EU______________________";
        partsWithDecoded = [];
        foreach (var part in word.PartsWithPattern)
        {
            if (word.IsPossible(part))
            {
                word.IsPossible(part);
                partsWithDecoded.Add(part);
            }
        }
        Assert.AreEqual(3, partsWithDecoded.Count);
        Assert.AreEqual("THE", partsWithDecoded[0]);
        Assert.AreEqual("TIE", partsWithDecoded[1]);
        Assert.AreEqual("TOE", partsWithDecoded[2]);
    }
}