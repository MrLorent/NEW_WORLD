using System;
using UnityEngine;

public enum AttributType
{
    TRUNK,
    BARK,
    FLOWER_SHAPE,
    FLOWER_COLOR
}

/// <summary>
/// Keeping all relevant information about a unit on a scriptable means we can gather and show
/// info on the menu screen, without instantiating the unit prefab.
/// </summary>
public abstract class Attribut : MonoBehaviour
{
    [SerializeField]
    protected AttributType _type;

    [SerializeField]
    protected Statistics _stats;

    protected virtual void Awake()
    {
        _stats.temperature = UnityEngine.Random.Range(0f, 100f);
        _stats.humidity = UnityEngine.Random.Range(0f, 100f);
    }
}

/// <summary>
/// Keeping base stats as a struct on the scriptable keeps it flexible and easily editable.
/// We can pass this struct to the spawned prefab unit and alter them depending on conditions.
/// </summary>
[Serializable]
public struct Statistics
{
    [Range(0.0f, 100.0f)]
    public float temperature;

    [Range(0.0f, 100.0f)]
    public float humidity;
}