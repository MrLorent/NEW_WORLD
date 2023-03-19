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

    public static float TEMP_MIN = 0;
    public static float TEMP_MAX = 0;

    [Range(-50.0F, 50.0F)]
    public float temperature;

    public static float HUM_MIN = 0;
    public static float HUM_MAX = 0;

    [Range(0.0F, 100.0F)]
    public float humidity_rate;
}