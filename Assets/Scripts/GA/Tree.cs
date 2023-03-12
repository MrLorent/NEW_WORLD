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

    private void Awake()
    {
        _lsystem = GetComponent<LSystem>();
    }

    private void Start()
    {
        init();
    }

    public void init()
    {
        _trunk = GAManager.Instance.get_random_trunk();
        _bark = GAManager.Instance.get_random_bark();

        _trunk_container.GetComponent<MeshRenderer>().material = _bark.material;
        _lsystem.Init(_trunk.lsystem_base);
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }
}
