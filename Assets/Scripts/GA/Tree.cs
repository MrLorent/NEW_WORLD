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

    public float fitness_score;// { get; private set; }

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

        compute_fitness_score();

        _lsystem.Init(trunk.lsystem_base, foliage_shape);
    }

    public void init(Trunk t, Bark b, FoliageShape fs, FoliageColor fc)
    {
        trunk = t;
        bark = b;
        foliage_shape = fs;
        foliage_color = fc;

        _trunk_container.GetComponent<MeshRenderer>().material = bark.material;
        _foliage_container.GetComponent<MeshRenderer>().material = foliage_color.material;

        compute_fitness_score();

        _lsystem.Init(trunk.lsystem_base, foliage_shape);
    }

    public void grow()
    {
        if(_lsystem.iterations <6)
        {
        _lsystem.iterations++;
        }
        _lsystem.Draw();
    }

    private void compute_fitness_score()
    {
        int x = (int) (transform.position.x / Cell.dimensions.x);
        int z = (int) (transform.position.z / Cell.dimensions.y);

        float environment_temperature = TerrainManager.Instance.grid[x][z].temperature;
        float environment_humidity_rate = TerrainManager.Instance.grid[x][z].humidity_rate;

        //compute the fitness score of the tree using tunk, bark, foliage_shape and foliage_color attributs indepedently and then average them
        float tree_temperature = (trunk.temperature + bark.temperature + foliage_shape.temperature + foliage_color.temperature) / 4.0F;
        float tree_humidity_rate = (trunk.humidity_rate + bark.humidity_rate + foliage_shape.humidity_rate + foliage_color.humidity_rate) / 4.0F;

        float temperature_difference = Mathf.Abs(environment_temperature - tree_temperature);
        float humidity_difference = Mathf.Abs(environment_humidity_rate - tree_humidity_rate);

        float temperature_fitness = 1.0F / temperature_difference * 100;
        float humidity_fitness = 1.0F / humidity_difference * 100;

        fitness_score = (temperature_fitness + humidity_fitness) / 2.0F;

    }
}
