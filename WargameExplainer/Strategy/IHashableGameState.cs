namespace WargameExplainer.Strategy;

/// <summary>
///     Interface that additionally flags, if we are currently in a hashable state.
/// </summary>
public interface IHashableGameState
{
    /// <summary>
    ///     Checks if we are currently in a state that is hashable.
    /// </summary>
    bool IsCurrentlyHashable { get; }

    /// <summary>
    ///     Called to append the hash information to the system
    /// </summary>
    /// <param name="writer">The binary writer, we append our data to.</param>
    public void AppendData(BinaryWriter writer);
}