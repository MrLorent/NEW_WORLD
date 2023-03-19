using UnityEngine;

public enum AttributType
{
    TRUNK,
    BARK,
    FOLIAGE_SHAPE,
    FOLIAGE_COLOR
}

public abstract class Attribut : ScriptableObject
{
    [Header("ALGO GEN PARAMS")]
    public AttributType attribut_type;

    [Range(-50.0F, 50.0F)]
    public float temperature;

    [Range(0.0F, 100.0F)]
    public float humidity_rate;
}