using Newtonsoft.Json.Linq;

namespace WargameExplainer.TrainingSystem;

/// <summary>
///     Contains a record with a sequence of observations and the final values that are  generated.
/// </summary>
public class EpisodicRecord
{
    /// <summary>
    ///     These are the values, that have been reached at the end of the episode.
    /// </summary>
    public float[] TargetValues { get; set; } = Array.Empty<float>();


    /// <summary>
    ///     The features we have observed.  First Dimension is the move made and second dimension float values of the features.
    /// </summary>
    public List<float[]> ObservedFeatureValues { get; set; } = new List<float[]>();


    /// <summary>
    /// Reads in a episodic record from a network stream.
    /// </summary>
    /// <param name="reader">Reader to read from</param>
    public void ReadNetworkData(BinaryReader reader)
    {
        int amountOfTargetValues = reader.ReadInt32();
        int amountOfEpisodes = reader.ReadInt32();
        int amountOfObservations = reader.ReadInt32();

        TargetValues = new float[amountOfTargetValues];
        for (int i = 0; i < amountOfTargetValues; ++i)
            TargetValues[i] = reader.ReadSingle();

        ObservedFeatureValues.Clear();
        for (int i = 0; i < amountOfEpisodes; ++i)
        {
            float[] target = new float[amountOfObservations];
            for (int j = 0; j < amountOfObservations; ++j)
                target[j] = reader.ReadSingle();

            ObservedFeatureValues.Add(target);
        }
    }


    /// <summary>
    /// Writes out all values onto the network.
    /// </summary>
    /// <param name="writer">Writer to write data to.</param>
    public void WriteNetworkData(BinaryWriter writer)
    {
        // First the array dimensions.
        writer.Write(TargetValues.Length);
        writer.Write(ObservedFeatureValues.Count);
        writer.Write(ObservedFeatureValues[0].Length);

        // Now the data itself.
        foreach (float value in TargetValues)
            writer.Write(value);

        // Now all observed feature values.
        foreach (float[] featureValues in ObservedFeatureValues)
        foreach (float value in featureValues)
            writer.Write(value);
    }
}