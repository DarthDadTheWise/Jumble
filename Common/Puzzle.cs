using System.ComponentModel;
using System.Diagnostics;

namespace Common;

[DebuggerDisplay("{Decoded}")]
public class Puzzle
{
    private readonly List<Letter> letters = [];
    private readonly List<Word> words = [];
    private string decoded = string.Empty;

    public Puzzle(string encoded)
    {
        // Prepare and validate input
        Encoded = encoded.Trim().ToUpper();

        // Letters
        foreach (var ch in Encoded)
        {
            var letter = Key[ch];
            if (char.IsLetter(ch))
            {
                letter.PropertyChanged += Letter_PropertyChanged;
            }
            letters.Add(letter);
        }

        // Unique words
        List<string> uniqueWords = [];
        var parts = SplitParts(Encoded);
        foreach (var part in parts)
        {
            if (!uniqueWords.Contains(part))
            {
                uniqueWords.Add(part);
                words.Add(new Word(Key, part));
            }
        }
    }

    private void Letter_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        decoded = string.Empty;
    }

    public JumbleKey Key { get; private set; } = new JumbleKey();

    public bool IsSolved
    {
        get
        {
            foreach (var word in words)
            {
                if (!word.IsSolved) return false;
            }
            return true;
        }
    }

    public bool IsSolvable
    {
        get
        {
            foreach (var word in words)
            {
                if (!word.IsSolved)
                {
                    if (0 == word.PossibleParts.Count) return false;
                }
            }
            return true;
        }
    }

    public void Solve()
    {
        if (IsSolved) return;

        var previousDecoded = Decoded;
        List<Word> sortedWords = new(words.OrderBy(w => w.PossibleParts.Count));
        foreach (var word in sortedWords)
        {
            if (!word.IsSolved)
            {
                // Try each possibility in an alternate reality.
                List<string> possibleParts = new(word.PossibleParts);
                foreach (var part in possibleParts)
                {
                    var previousKey = Key.KeyString;
                    word.Decoded = part;

                    if (IsSolvable)
                    {
                        Solve();
                        if (IsSolved)
                        {
                            return;
                        }
                    }
                    Key.KeyString = previousKey;
                }
            }
        }

        if (previousDecoded != Decoded)
        {
            Solve();
        }
    }

    public string Encoded { get; private set; }

    public string Decoded
    {
        get
        {
            if (string.IsNullOrEmpty(decoded))
            {
                foreach (Letter letter in letters)
                {
                    decoded += letter.Decoded;
                }
            }
            return decoded;
        }
    }

    private static bool HasLetter(string source)
    {
        foreach (char ch in source)
        {
            if (char.IsLetter(ch)) return true;
        }
        return false;
    }

    private static List<string> SplitParts(string encoded)
    {
        encoded = encoded.Trim();
        List<string> parts = [];
        foreach (var part in encoded.Split([' ', '!', '\"', ',', '?', ';', '.', ':']))
        {
            var nonEmptyPart = part.Trim().Trim('\'');
            if (!string.IsNullOrEmpty(nonEmptyPart) && HasLetter(nonEmptyPart))
            {
                parts.Add(nonEmptyPart);
            }
        }
        return parts;
    }
}
