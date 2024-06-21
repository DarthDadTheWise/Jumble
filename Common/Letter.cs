using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace Common;

/// <summary>
/// A letter or symbol in the cryptogram.
/// </summary>
[DebuggerDisplay("{Encoded} ==> {Decoded}")]
public class Letter : ObservableObject
{
    private readonly char encoded;
    private char decoded = PLACEHOLDER;

    public Letter(char encoded)
    {
        // All letters are uppercase
        this.encoded = char.ToUpper(encoded);

        // Symbols have no substitution.
        if (!char.IsLetter(encoded))
        {
            decoded = encoded;
        }
    }

    /// <summary>
    /// Used when the is no substitution for this letter.
    /// </summary>
    public const char PLACEHOLDER = '_';

    /// <summary>
    /// True when there is a substitution for this letter. Otherwise false.
    /// </summary>
    public bool IsSolved { get { return PLACEHOLDER != decoded; } }

    /// <summary>
    /// The letter or symbol to decode.
    /// </summary>
    public char Encoded { get { return encoded; } }

    /// <summary>
    /// The english decoding for this letter.
    /// </summary>
    public char Decoded
    {
        get { return decoded; }
        set
        {
            // All letters are uppercase.
            value = char.ToUpper(value);
            if (decoded == value) return;

            // Cannot decode to the same character
            if (encoded == value) return;

            OnPropertyChanging(nameof(Decoded));
            decoded = value;
            OnPropertyChanged(nameof(Decoded));
        }
    }
}
