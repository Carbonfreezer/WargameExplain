using System.Diagnostics;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace WargameExplainer.TrainingSystem;

/// <summary>
///     Class thatz fits a ridge regulated  linear model to the observed data.
/// </summary>
public class RidgeModelFitter
{
    /// <summary>
    ///     The context handle of the machine learning system.
    /// </summary>
    private readonly MLContext m_context;


    /// <summary>
    ///     The used regression pipeline.
    /// </summary>
    private readonly SdcaRegressionTrainer m_pipeline;


    /// <summary>
    ///     The schema we use.
    /// </summary>
    private readonly SchemaDefinition m_schemaDefinition;

    /// <summary>
    ///     Generates the fitter.
    /// </summary>
    /// <param name="trainingInfo">The provider with the training information.</param>
    public RidgeModelFitter(TrainingInfoProvider trainingInfo)
    {
        m_context = new MLContext(0);

        m_schemaDefinition = SchemaDefinition.Create(typeof(DataPoint));
        m_schemaDefinition["Features"].ColumnType =
            new VectorDataViewType(NumberDataViewType.Single, trainingInfo.NumInputFeatures);

        m_pipeline = m_context.Regression.Trainers.Sdca(l2Regularization: trainingInfo.L2RegularizationParameter > 0.0001f ? trainingInfo.L2RegularizationParameter : null);
    }


    /// <summary>
    ///     Generates an ML data view from the episodes.
    /// </summary>
    /// <param name="episodes">The episode buffer we want to generatre the data from.</param>
    /// <param name="playerPerspective">The player perspective we are analyzing.</param>
    /// <returns>The data view to use.</returns>
    private IDataView PrepareTrainingData(IList<EpisodicRecord> episodes, int playerPerspective)
    {
        List<DataPoint> inputList = new List<DataPoint>();
        foreach (EpisodicRecord episode in episodes)
        {
            float destinationValue = episode.TargetValues[playerPerspective];

            foreach (float[] values in episode.ObservedFeatureValues)
            {
                DataPoint newPoint = new DataPoint
                {
                    Features = values,
                    Label = destinationValue
                };

                inputList.Add(newPoint);
            }
        }

        IDataView trainingData = m_context.Data.LoadFromEnumerable(inputList, m_schemaDefinition);
        return trainingData;
    }


    /// <summary>
    ///     Generates the complete regularized model from the episodic list.
    /// </summary>
    /// <param name="episodes">Episodic list.</param>
    /// <returns>Generalized model.</returns>
    public LinearModel GetRegularizedModel(IList<EpisodicRecord> episodes)
    {
        int numPerspectives = episodes[0].TargetValues.Length;
        List<List<float>> weights = new List<List<float>>();
        List<float> intercept = new List<float>();

        for (int perspective = 0; perspective < numPerspectives; ++perspective)
        {
            IDataView trainingData = PrepareTrainingData(episodes, perspective);
            LinearRegressionModelParameters? model = m_pipeline.Fit(trainingData).Model;

            Debug.Assert(model != null, "We expect a model at this point.");
            weights.Add(model.Weights.ToList());
            intercept.Add(model.Bias);
        }


        return new LinearModel { Weights = weights, Intercept = intercept };
    }

    /// <summary>
    ///     The data point needed for the ML System of microsoft.
    /// </summary>
    private class DataPoint
    {
        /// <summary>
        ///     What we want to get out.
        /// </summary>
        public float Label { get; set; }


        /// <summary>
        ///     The resulting vector type.
        /// </summary>
        public float[] Features { get; set; } = Array.Empty<float>();
    }
}