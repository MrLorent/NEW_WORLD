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
    [Header("ID")]
    [SerializeField]
    public string id;

    [Header("ALGO GEN PARAMS")]
    [SerializeField]
    public AttributType attribut_type;

    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float temperature;

    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float humidity;
}