namespace WargameExplainer.Strategy;

/// <summary>
///     Interface to hash be able to hash with the CRC64comparer.
///     This is only used as a simple reminder for resharper to add the functionality.
/// </summary>
public interface IHashable
{
    /// <summary>
    ///     Called to append the hash information to the system
    /// </summary>
    /// <param name="writer">The binary writer, we append our data to.</param>
    public void AppendData(BinaryWriter writer);
}