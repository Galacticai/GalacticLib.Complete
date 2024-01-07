using System.Security.Cryptography;
using System.Text;

namespace GalacticLib.Security;

public class OTP : IDisposable {
    #region this object

    public byte[] Key { get; private set; }
    public int KeyLength { get; }
    private long LifeSpanMS { get; }
    public bool Regenerate { get; }
    public bool IsExpired { get; private set; }

    private readonly Timer _Timer;

#pragma warning disable CS8618 
    public OTP(int keyLength, TimeSpan lifeSpan, bool regenerate) {
        if (keyLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(keyLength), "Key length must be positive");
        if (lifeSpan <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(lifeSpan), "Life span must be positive");

        KeyLength = keyLength;
        RegenerateKeySilently(keyLength);
        Regenerate = regenerate;
        LifeSpanMS = (long)lifeSpan.TotalMilliseconds;
        _Timer = new(_ => OnTick(), null, LifeSpanMS, Regenerate ? LifeSpanMS : Timeout.Infinite);
    }
#pragma warning restore CS8618

    #endregion
    #region events

    public delegate void RegenegatedHandler(OTP sender, byte[] newKey, DateTime generatedAt, TimeSpan lifeSpan);
    /// <summary> The <see cref="Key"/> was regenerated at the specified time and has the specified lifespan </summary>
    public event RegenegatedHandler? Regenerated;

    public delegate void ExpiredHandler(OTP sender, byte[] oldKey, DateTime expiredAt);
    /// <summary> The <see cref="Key"/> has expired at the specified time </summary>
    public event ExpiredHandler? Expired;

    #endregion
    #region Shortcuts

    public TimeSpan LifeSpan => TimeSpan.FromMilliseconds(LifeSpanMS);

    #endregion
    #region Methods

    protected virtual void OnTick() {
        if (Regenerate) {
            RegenerateKey(Key.Length);
        } else {
            IsExpired = true;
            _Timer.Dispose();
            Expired?.Invoke(this, Key, DateTime.Now);
        }
    }

    private void RegenerateKeySilently(int keyLength) {
        Key = new byte[keyLength];
        RandomNumberGenerator.Fill(Key);
    }
    protected virtual void RegenerateKey(int keyLength) {
        RegenerateKeySilently(keyLength);
        Regenerated?.Invoke(this, Key, DateTime.Now, LifeSpan);
    }

    /// <summary> Compare a <paramref name="key"/> against the current <see cref="Key"/> </summary>
    public bool Verify(byte[] key) => Key.SequenceEqual(key);
    /// <summary> Compare a <paramref name="hex"/> <see cref="string"/> against the current <see cref="Key"/> </summary>
    public bool Verify(string hex) {
        try {
            return Verify(Convert.FromHexString(hex));
        } catch { return false; }
    }


    #endregion
    #region Overrides
    /// <summary> Get the <see cref="Key"/> as a hexadecimal <see cref="string"/> and <paramref name="separateEvery"/> amount of <see cref="byte"/>s using <paramref name="separator"/> <see cref="char"/> </summary>
    /// <param name="separateEvery"> <see cref="byte"/>s count before inserting <paramref name="separator"/> </param>
    /// <param name="separator"> <see cref="char"/> used as a separator </param>
    /// <returns> Example: 9A0F-D68E-8402 </returns>
    public string ToString(int separateEvery = 2, char separator = '-') {
        StringBuilder sb = new();
        for (int i = 0; i < Key.Length; i++) {
            sb.Append(Key[i]);
            if ((i + 1) % separateEvery == 0 && i != Key.Length - 1)
                sb.Append(separator);
        }
        return sb.ToString();
    }
    /// <summary> Get the <see cref="Key"/> as a hexadecimal <see cref="string"/> </summary>
    /// <returns> Example: 9A0FD68E8402 </returns>
    public override string ToString() => Convert.ToHexString(Key);

    public void Dispose() {
        _Timer.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}
