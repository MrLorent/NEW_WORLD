using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    /*======== PUBLIC ========*/
    [Header("3D MODEL")]
    [SerializeField] private Transform _trunk_container;
    [SerializeField] private Transform _foliage_container;

    public float fitness_score; //{ get; private set; }

    /*======== PRIVATE ========*/
    [Header("LSYSTEM")]
    private LSystem _lsystem;

    public Trunk trunk; //{ get; private set; }
    public Bark bark; // { get; private set; }
    public FoliageShape foliage_shape; // { get; private set; }
    public FoliageColor foliage_color; // { get; private set; }

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
        if(_lsystem.iterations < 7)
        {
            _lsystem.iterations++;
            _lsystem.Draw();
        }
        else
        {
            transform.localScale *= 1.1F;
        }
    }

    public Quaternion get_foliage_orientation(Quaternion turtle)
    {
        return turtle;
    }

    private void compute_fitness_score()
    {
        int x = (int) (transform.position.x / Cell.dimensions.x);
        int z = (int) (transform.position.z / Cell.dimensions.y);

        float environment_temperature = TerrainManager.Instance.grid[x][z].temperature;
        float environment_humidity_rate = TerrainManager.Instance.grid[x][z].humidity_rate;

        float trunk_coef = 0.4F;
        float bark_coef = 0.1F;
        float foliage_shape_coef = 0.3F;
        float foliage_color_coef = 0.2F;

        float tree_temperature = (trunk.temperature * trunk_coef + bark.temperature * bark_coef + foliage_shape.temperature * foliage_shape_coef + foliage_color.temperature * foliage_color_coef) / 4.0F;
        float temperature_fit_rate = 1 - Mathf.Abs(environment_temperature - tree_temperature) / (Attribut.TEMP_MAX - Attribut.TEMP_MIN);

        float tree_humidity_rate = (trunk.humidity_rate * trunk_coef + bark.humidity_rate * bark_coef + foliage_shape.humidity_rate * foliage_shape_coef + foliage_color.humidity_rate * foliage_color_coef) / 4.0F;
        float humidity_fit_rate = 1 - Mathf.Abs(environment_humidity_rate - tree_humidity_rate) / (Attribut.HUM_MAX - Attribut.HUM_MIN);

        fitness_score = (temperature_fit_rate + humidity_fit_rate) / 2.0F;
    }
}
