using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    /*======== PUBLIC ========*/
    [Header("3D MODEL")]
    [SerializeField] private Transform _trunk_container;
    [SerializeField] private Transform _foliage_container;

    public Material material;
    public float fitnessScore = 1.0f;
    public Color color = new Color(1.0f, 1.0f, 1.0f);

    /*======== PRIVATE ========*/
    [Header("LSYSTEM")]
    private LSystem _lsystem;

    [Header("GA ATTRIBUTS")]
    private Trunk _trunk;
    private Bark _bark;
    private FoliageShape _foliage_shape;
    private FoliageColor _foliage_color;

    private void Awake()
    {
        _lsystem = GetComponent<LSystem>();
    }

    public void init()
    {
        _trunk = GAManager.Instance.get_random_trunk();
        _bark = GAManager.Instance.get_random_bark();
        _foliage_shape = GAManager.Instance.get_random_foliage_shape();
        _foliage_color = GAManager.Instance.get_random_foliage_color();

        _trunk_container.GetComponent<MeshRenderer>().material = _bark.material;
        _foliage_container.GetComponent<MeshRenderer>().material = _foliage_color.material;

        _lsystem.Init(_trunk.lsystem_base, _foliage_shape.foliage_prefab);
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }
}
