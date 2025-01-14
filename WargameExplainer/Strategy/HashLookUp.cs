using System.IO.Hashing;

namespace WargameExplainer.Strategy;

/// <summary>
///     A Hash map for node game states.
/// </summary>
public class HashLookUp : IDisposable
{
    /// <summary>
    ///     The current filling of the hash table entries.
    /// </summary>
    private readonly int[] m_fillingHashValues;

    /// <summary>
    ///     We go for a 64 bit hasher here.
    /// </summary>
    private readonly Crc64 m_hasher = new();

    /// <summary>
    ///     The granularity we have in the hash table.
    /// </summary>
    private readonly int m_hashGranularity;

    /// <summary>
    ///     The list with the hash values.
    /// </summary>
    private readonly List<HashEntry>[] m_hashLists;


    /// <summary>
    ///     Contains the memory stream data.
    /// </summary>
    private readonly MemoryStream m_memStream;


    /// <summary>
    ///     The writer to ur memory stream.
    /// </summary>
    private readonly BinaryWriter m_writer;


    /// <summary>
    ///     Generates the hash look up table.
    /// </summary>
    /// <param name="hashGranularity">The granularity we want to use in the hash table.</param>
    public HashLookUp(int hashGranularity)
    {
        m_hashGranularity = hashGranularity;
        m_hashLists = new List<HashEntry>[hashGranularity];
        m_fillingHashValues = new int[hashGranularity];
        for (int i = 0; i < hashGranularity; ++i)
            m_hashLists[i] = new List<HashEntry>(5);
        m_memStream = new MemoryStream();
        m_writer = new BinaryWriter(m_memStream);
    }

    /// <summary>
    ///     Asks for the average filling of the slots.
    /// </summary>
    public double AverageFilling => m_fillingHashValues.Average();

    /// <summary>
    ///     Clears the memory resources of the hashing mechanism.
    /// </summary>
    public void Dispose()
    {
        m_writer.Dispose();
        m_memStream.Dispose();
    }


    /// <summary>
    ///     Gets the hashcode of a complete game state.
    /// </summary>
    /// <param name="node">Implementation of node element.</param>
    /// <returns>Hash code.</returns>
    public ulong GetHashCode(IHashableGameState node)
    {
        m_writer.Seek(0, SeekOrigin.Begin);
        m_memStream.Seek(0, SeekOrigin.Begin);
        node.AppendData(m_writer);
        m_memStream.Seek(0, SeekOrigin.Begin);
        m_hasher.Append(m_memStream);
        ulong result = m_hasher.GetCurrentHashAsUInt64();
        m_hasher.Reset();
        return result;
    }

    /// <summary>
    ///     Returns the bucket that corresponds to the hash value.
    /// </summary>
    /// <param name="hashValue">Hash value to probe.</param>
    /// <returns>Bucket to return.</returns>
    private int GetBucket(ulong hashValue)
    {
        return (int)(hashValue % (ulong)m_hashGranularity);
    }

    /// <summary>
    ///     Tries to get the evaluation with the indicated hash value.
    /// </summary>
    /// <param name="hashValue">Hash value to probe for.</param>
    /// <param name="evaluations">Span of evaluations that will get filled.</param>
    /// <returns>True if value was found otherwise false.</returns>
    public bool TryGet(ulong hashValue, in Span<float> evaluations)
    {
        int bucket = GetBucket(hashValue);

        for (int i = 0; i < m_fillingHashValues[bucket]; ++i)
            if (m_hashLists[bucket][i].m_hashValue == hashValue)
            {
                for (int k = 0; k < m_hashLists[bucket][i].m_evaluations.Length; ++k)
                    evaluations[k] = m_hashLists[bucket][i].m_evaluations[k];

                return true;
            }

        return false;
    }


    /// <summary>
    ///     Inserts a new entry into the hashmap, which is a span of evaluations.
    /// </summary>
    /// <param name="hashValue">The hash value of the game situation.</param>
    /// <param name="evaluations">The span with the evaluations to insert.</param>
    public void InsertIntoHashmap(ulong hashValue, in Span<float> evaluations)
    {
        int bucket = GetBucket(hashValue);

        int currentFilling = m_fillingHashValues[bucket];
        if (currentFilling < m_hashLists[bucket].Count)
        {
            // In this case we can recycle an existing entry.
            m_hashLists[bucket][currentFilling].m_hashValue = hashValue;
            for (int i = 0; i < evaluations.Length; ++i)
                m_hashLists[bucket][currentFilling].m_evaluations[i] = evaluations[i];
        }
        else
        {
            // In this case we need a new entry.
            HashEntry newEntry = new HashEntry
            {
                m_evaluations = evaluations.ToArray(),
                m_hashValue = hashValue
            };
            m_hashLists[bucket].Add(newEntry);
        }

        m_fillingHashValues[bucket]++;
    }

    /// <summary>
    ///     Empties the complete hashmap. Can be used in strategies like
    ///     Minimax, that can not recycle the information.
    /// </summary>
    public void Flush()
    {
        for (int i = 0; i < m_hashGranularity; ++i)
            m_fillingHashValues[i] = 0;
    }

    /// <summary>
    ///     Represents an entry in our hash list, consists of the hashing value and the stored evaluations.
    /// </summary>
    private class HashEntry
    {
        /// <summary>
        ///     The array with the evaluations.
        /// </summary>
        public float[] m_evaluations = Array.Empty<float>();

        /// <summary>
        ///     The ash value we store.
        /// </summary>
        public ulong m_hashValue;
    }
}