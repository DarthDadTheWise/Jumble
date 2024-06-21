using System.ComponentModel;
using System.Diagnostics;

namespace Common;

public class JumbleKey
{
    private readonly Dictionary<char, Letter> alphabet = [];

    private readonly Dictionary<char, Letter> litterals = [];

    private readonly Dictionary<char, bool> used = [];

    private string keyString = string.Empty;

    public JumbleKey()
    {
        for (char ch = 'A'; ch <= 'Z'; ch++)
        {
            Letter letter = new(ch);
            letter.PropertyChanging += Letter_PropertyChanging;
            letter.PropertyChanged += Letter_PropertyChanged;
            alphabet.Add(ch, letter);

            used.Add(ch, false);
        }
    }

    private void Letter_PropertyChanging(object? sender, PropertyChangingEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Decoded":
                if (sender is Letter changedLetter)
                {
                    if (!char.IsLetter(changedLetter.Decoded)) return;
                    used[changedLetter.Decoded] = false;
                }
                break;
        }
    }

    private void Letter_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Decoded":
                if (sender is Letter changedLetter)
                {
                    keyString = string.Empty;
                    if (!char.IsLetter(changedLetter.Decoded)) return;
                    foreach (var letter in alphabet.Values)
                    {
                        if (letter.Encoded != changedLetter.Encoded)
                        {
                            if (letter.Decoded == changedLetter.Decoded)
                            {
                                letter.Decoded = Letter.PLACEHOLDER;
                            }
                        }
                    }
                    used[changedLetter.Decoded] = true;
                }
                break;
        }
    }

    public string KeyString
    {
        get
        {
            if (string.IsNullOrEmpty(keyString))
            {
                foreach (var letter in alphabet.Values)
                {
                    keyString += letter.Decoded;
                }
            }
            return keyString;
        }
        set
        {
            Debug.Assert(value.Length == 26);
            int offset = Convert.ToInt32('A');
            for (var i = 0; i < value.Length; i++)
            {
                char ch = Convert.ToChar(offset + i);
                alphabet[ch].Decoded = value[i];
            }
            keyString = value;
        }
    }

    // Indexer declaration
    public Letter this[char ch]
    {
        get
        {
            // When a letter, return the letter
            ch = char.ToUpper(ch);
            if (char.IsLetter(ch)) return alphabet[ch];

            // When a symbol, return the symbol and create it if necessary
            if (!litterals.ContainsKey(ch))
                litterals.Add(ch, new Letter(ch));
            return litterals[ch];
        }
    }

    public List<Letter> this[string encoded]
    {
        get
        {
            List<Letter> letters = [];
            foreach (var ch in encoded.ToUpper())
            {
                letters.Add(this[ch]);
            }
            return letters;
        }
    }

    public bool IsLetterUsed(char ch)
    {
        if (!char.IsLetter(ch)) return false;
        return used[ch];
    }
}

