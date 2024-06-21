using System.ComponentModel;
using System.Diagnostics;

namespace Common;

/// <summary>
/// An english word to decode in the cryptogram
/// </summary>
[DebuggerDisplay("{Encoded} => {Decoded}")]
public class Word
{
    /// <summary>
    /// The substitutions for the entire alphabet.
    /// </summary>
    private readonly JumbleKey key;

    /// <summary>
    /// The english decoding for this word. The word
    /// is only decoded when requested.
    /// </summary>
    private string decoded = string.Empty;

    /// <summary>
    /// All english words that match this word's pattern.
    /// </summary>
    private List<string>? partsForPattern;

    /// <summary>
    /// All english words that match the decoding so far.
    /// </summary>
    private List<string>? possibleParts;

    /// <summary>
    /// All words share the same letter instances.
    /// </summary>
    /// <param name="key">The decoder ring</param>
    /// <param name="encoded">The letters and symbols for this word</param>
    public Word(JumbleKey key, string encoded)
    {
        this.key = key;
        Encoded = encoded.ToUpper();
        for (char ch = 'A'; ch <= 'Z'; ch++)
        {
            var letter = key[ch];
            if (Encoded.Contains(ch))
            {
                letter.PropertyChanged += EncodedLetter_PropertyChanged;
            }
            else
            {
                letter.PropertyChanging += OtherLetter_PropertyChanging;
            }
        }
        Letters = key[Encoded];
        Pattern = MakePattern(Encoded);
    }

    private void OtherLetter_PropertyChanging(object? sender, PropertyChangingEventArgs e)
    {
        if (null == possibleParts) return;
        switch (e.PropertyName)
        {
            case "Decoded":
                if (sender is Letter letter)
                {
                    if (char.IsLetter(letter.Decoded))
                    {
                        if (Decoded.Contains(letter.Decoded))
                        {
                            possibleParts = null;
                        }
                    }
                }
                break;
        }
    }

    private void EncodedLetter_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // decode this word only when requested.
        decoded = string.Empty;
        possibleParts = null;
    }

    /// <summary>
    /// Construct the pattern for the supplied word.
    /// From left to right, replace each supplied letter with A, B, C, etc.
    /// Example: 
    ///   "THE" returns "ABC"
    ///   "ALL" returns "ABB"
    ///   "I'M" returns "A'B"
    /// </summary>
    /// <param name="part">The letters for the pattern</param>
    /// <returns>A pattern for the word</returns>
    public static string MakePattern(string part)
    {
        string pattern = part.ToLower();
        int i = Convert.ToInt32('A');
        foreach (var ch in pattern.Distinct())
        {
            if (char.IsLetter(ch))
            {
                var replacement = Convert.ToChar(i++);
                pattern = pattern.Replace(ch, replacement);
            }
        }
        return pattern;
    }

    /// <summary>
    /// The english substitutions for this word.
    /// </summary>
    public string Decoded
    {
        get
        {
            if (string.IsNullOrEmpty(decoded))
            {
                foreach (Letter letter in Letters)
                {
                    decoded += letter.Decoded;
                }
            }
            return decoded;
        }
        set
        {
            Debug.Assert(value.Length == Letters.Count);
            if (value.Length != Letters.Count) return;
            for (int i = 0; i < value.Length; i++)
            {
                var ch = value[i];
                var letter = Letters[i];
                letter.Decoded = ch;
            }
            decoded = value;
        }
    }

    /// <summary>
    /// The word to decode.
    /// </summary>
    public string Encoded { get; private set; }

    /// <summary>
    /// The letters to decode.
    /// </summary>
    public List<Letter> Letters { get; private set; }

    /// <summary>
    /// The pattern for this word.
    /// </summary>
    public string Pattern { get; private set; }

    /// <summary>
    /// All English words that have the same pattern as this word's pattern.
    /// </summary>
    public List<string> PartsWithPattern
    {
        get
        {
            if (null == partsForPattern)
            {
                //partsForPattern = [];
                if (English.PartsByPattern.TryGetValue(Pattern, out List<string>? value))
                {
                    partsForPattern = value;
                    //foreach (var part in value)
                    //{
                    //    if (IsPossible(part))
                    //    {
                    //        partsForPattern.Add(part);
                    //    }
                    //}
                }
                else
                {
                    partsForPattern = [];
                }
            }
            return partsForPattern;
        }
    }

    private Dictionary<string, List<string>> cache = [];

    /// <summary>
    /// All English words that match this word's decoding so far.
    /// </summary>
    public List<string> PossibleParts
    {
        get
        {
            if (null == possibleParts)
            {
                if (IsSolved)
                {
                    possibleParts = [];
                    if (IsPossible(Decoded))
                    {
                        possibleParts.Add(Decoded);
                    }
                }
                else
                {
                    if (cache.TryGetValue(key.KeyString, out List<string>? value))
                    {
                        possibleParts = value;
                    }
                    else
                    {
                        possibleParts = [];
                        foreach (var part in PartsWithPattern)
                        {
                            if (IsPossible(part))
                            {
                                possibleParts.Add(part);
                            }
                        }
                        cache.Add(key.KeyString, possibleParts);
                    }
                }
            }
            ArgumentNullException.ThrowIfNull(possibleParts);
            return possibleParts;
        }
    }

    /// <summary>
    /// Is this english word a possible solution for this word?
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public bool IsPossible(string part)
    {
        if (Encoded.Length != part.Length) return false;
        for (int i = 0; i < part.Length; i++)
        {
            var ch = part[i];
            if (Letter.PLACEHOLDER == ch) return false;

            var letter = Letters[i];
            if (letter.IsSolved)
            {
                if (ch != letter.Decoded) return false;
            }
            else
            {
                // cannot assign same letter to itself
                if (!char.IsLetter(ch)) return false;
                if (letter.Encoded == ch) return false;
                if (key.IsLetterUsed(ch)) return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Does every letter in this word have a substitue letter?
    /// </summary>
    public bool IsSolved
    {
        get
        {
            if (0 == Letters.Count) return false;
            foreach (var letter in Letters)
            {
                if (!letter.IsSolved) return false;
            }
            return true;
        }
    }
}
