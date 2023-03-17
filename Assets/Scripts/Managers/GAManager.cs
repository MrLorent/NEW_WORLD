using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAManager : Singleton<GAManager>
{
    [SerializeField]
    private AttributsIndex attributs_index;

    [SerializeField]
    private GameObject _tree_prefab;

    [SerializeField]
    private uint _start_population_size = 50;

    [SerializeField]
    private Transform _trees_container;
    private List<Tree> _trees_population;

    [SerializeField]
    private float mutation_rate = 0.05F;

    protected override void Awake()
    {
        base.Awake();
        _trees_population = new List<Tree>();
    }

    private void Start()
    {
        for(uint i = 0; i < _start_population_size; i++)
        {
            Tree new_tree = Instantiate(
                _tree_prefab,
                TerrainManager.Instance.get_random_position(),
                Quaternion.identity,
                _trees_container
            ).GetComponent<Tree>();

            new_tree.init();

            _trees_population.Add(new_tree);
        }

        InvokeRepeating("next_generation", 0.0F, 10.0F);
    }

    private void next_generation()
    {
        Selection();
        Reproduction();
    }

    private void Selection()
    {
        _trees_population.Sort(
            delegate (Tree a, Tree b)
            {
                if (a.fitness > b.fitness)
                {
                    return 1;
                }
                else if (a.fitness < b.fitness)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        );

        int amonth_to_kill = (_trees_population.Count / 3) % 2 != 0 ? (_trees_population.Count / 3) + 1 : (_trees_population.Count / 3); ;
        
        for(int i = _trees_population.Count - amonth_to_kill; i < _trees_population.Count; i++)
        {
            _trees_population[i].transform.Destroy();
        }

        _trees_population.RemoveRange(_trees_population.Count - amonth_to_kill, amonth_to_kill);
    }

    private void Reproduction()
    {
        List<Tree> childs = new List<Tree>();

        for(int i = 1;  i < _trees_population.Count; i += 2)
        {
            Tree mother = _trees_population[i - 1];
            Tree father = _trees_population[i];

            mother.grow();
            father.grow();

            Trunk child_trunk = Random.Range(0, 100) > 50 ? mother.trunk : father.trunk;
            Bark child_bark = Random.Range(0, 100) > 50 ? mother.bark : father.bark;
            FoliageShape child_foliage_shape = Random.Range(0, 100) > 50 ? mother.foliage_shape : father.foliage_shape;
            FoliageColor child_foliage_color = Random.Range(0, 100) > 50 ? mother.foliage_color : father.foliage_color;

            if (Random.Range(0,1) < mutation_rate)
            {
                child_trunk = get_random_trunk();
            }

            if (Random.Range(0, 1) < mutation_rate)
            {
                child_bark = get_random_bark();
            }

            if (Random.Range(0, 1) < mutation_rate)
            {
                child_foliage_shape = get_random_foliage_shape();
            }

            if (Random.Range(0, 1) < mutation_rate)
            {
                child_foliage_color = get_random_foliage_color();
            }

            Tree child = Instantiate(
                _tree_prefab,
                TerrainManager.Instance.get_random_position(),
                Quaternion.identity,
                _trees_container
            ).GetComponent<Tree>();

            child.init(
                child_trunk,
                child_bark,
                child_foliage_shape,
                child_foliage_color
            );

            childs.Add(child);
        }

        _trees_population.AddRange(childs);
    }

    public Trunk get_random_trunk()
    {
        return attributs_index.trunks[Random.Range(0, attributs_index.trunks.Count)];
    }

    public Bark get_random_bark()
    {
        return attributs_index.barks[Random.Range(0, attributs_index.barks.Count)];
    }

    public FoliageShape get_random_foliage_shape()
    {
        return attributs_index.foliage_shapes[Random.Range(0, attributs_index.foliage_shapes.Count)];
    }

    public FoliageColor get_random_foliage_color()
    {
        return attributs_index.foliage_colors[Random.Range(0, attributs_index.foliage_colors.Count)];
    }
}
