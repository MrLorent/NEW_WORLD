using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Tree : MonoBehaviour
{
    /*======== PUBLIC ========*/
    [Header("3D MODEL")]
    [SerializeField] private Transform _trunk_container;
    [SerializeField] private Transform _foliage_container;

    public Material material;
    public float fitnessScore = 1.0f;
    public float fitness { get; private set; }
    public Color color = new Color(1.0f, 1.0f, 1.0f);

    /*======== PRIVATE ========*/
    [Header("LSYSTEM")]
    private LSystem _lsystem;

    public Trunk trunk { get; private set; }
    public Bark bark { get; private set; }
    public FoliageShape foliage_shape { get; private set; }
    public FoliageColor foliage_color { get; private set; }

    private void Awake()
    {
        _lsystem = GetComponent<LSystem>();
    }

    public void init()
    {
        trunk = GAManager.Instance.get_random_trunk();
        bark = GAManager.Instance.get_random_bark();
        foliage_shape = GAManager.Instance.get_random_foliage_shape();
        foliage_color = GAManager.Instance.get_random_foliage_color();

        _trunk_container.GetComponent<MeshRenderer>().material = bark.material;
        _foliage_container.GetComponent<MeshRenderer>().material = foliage_color.material;

        compute_fitness();

        _lsystem.Init(trunk.lsystem_base, foliage_shape.foliage_prefab);
    }

    public void init(Trunk t, Bark b, FoliageShape fs, FoliageColor fc)
    {
        trunk = t;
        bark = b;
        foliage_shape = fs;
        foliage_color = fc;

        _trunk_container.GetComponent<MeshRenderer>().material = bark.material;
        _foliage_container.GetComponent<MeshRenderer>().material = foliage_color.material;

        compute_fitness();

        _lsystem.Init(trunk.lsystem_base, foliage_shape.foliage_prefab);
    }

    public void grow()
    {
        _lsystem.iterations++;
        _lsystem.Draw();
    }

    private void compute_fitness()
    {
        int x = (int) (transform.position.x / Cell.dimensions.x);
        int z = (int) (transform.position.z / Cell.dimensions.y);

        float environment_temperature = TerrainManager.Instance.grid[x][z].temperature;
        float environment_humidity_rate = TerrainManager.Instance.grid[x][z].humidity_rate;

        float tree_temperature = (trunk.temperature + bark.temperature + foliage_shape.temperature + foliage_color.temperature) / 4.0F;
        float tree_humidity_rate = (trunk.humidity_rate + bark.humidity_rate + foliage_shape.humidity_rate + foliage_color.humidity_rate) / 4.0F;

        fitness = (environment_temperature - tree_temperature) + (environment_humidity_rate - tree_humidity_rate);
    }



    public void SetColor(Color color)
    {
        this.color = color;
    }
}
