namespace WargameExplainer.TrainingSystem;

/// <summary>
///     The linear model used in the regularized linear regression.
/// </summary>
public class LinearModel
{
    /// <summary>
    ///     The weight matrix of the model.
    /// </summary>
    public List<List<float>> Weights { get; set; } = new List<List<float>>();

    /// <summary>
    ///     The intercept value of the model.
    /// </summary>
    public List<float> Intercept { get; set; } = new List<float>();


    /// <summary>
    ///     Transforms the featureInput vector into a new one.
    /// </summary>
    /// <param name="featureInput">The length of the featureInput vector.</param>
    /// <param name="situationEvaluation">The span with the resulting values.</param>
    public void TransformInput(in Span<float> featureInput, in Span<float> situationEvaluation)
    {
        for (int player = 0; player < Intercept.Count; ++player)
        {
            float acc = Intercept[player];
            for (int j = 0; j < featureInput.Length; ++j)
                acc += featureInput[j] * Weights[player][j];
            situationEvaluation[player] = acc;
        }
    }

    /// <summary>
    ///     Write the data to the writer.
    /// </summary>
    /// <param name="writer">Binary writer to use.</param>
    public void WriteToStream(BinaryWriter writer)
    {
        writer.Write(Intercept.Count);
        foreach (float f in Intercept)
            writer.Write(f);

        writer.Write(Weights[0].Count);
        foreach (IList<float> list in Weights)
        foreach (float f in list)
            writer.Write(f);
    }

    /// <summary>
    ///     Saves the linear model to the indicated file.
    /// </summary>
    /// <param name="fileName">Filename to save data to.</param>
    public void SaveToFile(string fileName)
    {
        using BinaryWriter stream = new BinaryWriter(File.Open(fileName, FileMode.Create));
        WriteToStream(stream);
    }

    /// <summary>
    ///     Reads the linear model from the file.
    /// </summary>
    /// <param name="fileName"></param>
    public void LoadFromFile(string fileName)
    {
        using BinaryReader stream = new BinaryReader(File.Open(fileName, FileMode.Open));
        ReadFromStream(stream);
    }

    /// <summary>
    ///     Reads the data from a stream.
    /// </summary>
    /// <param name="reader">Reader to read from. </param>
    public void ReadFromStream(BinaryReader reader)
    {
        Intercept.Clear();
        int numPlayers = reader.ReadInt32();
        for (int i = 0; i < numPlayers; ++i)
            Intercept.Add(reader.ReadSingle());

        int numFeatures = reader.ReadInt32();
        Weights.Clear();
        for (int i = 0; i < numPlayers; ++i)
        {
            List<float> newList = new List<float>(numFeatures);
            for (int j = 0; j < numFeatures; ++j)
                newList.Add(reader.ReadSingle());
            Weights.Add(newList);
        }
    }
}