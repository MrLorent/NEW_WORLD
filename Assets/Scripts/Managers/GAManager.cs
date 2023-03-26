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
    private uint _start_population_size = 10;

    [SerializeField]
    private Transform _trees_container;

    //each biom has a list of trees that are in it
    private Dictionary<BiomType, List<Tree>> _trees_population;

    [SerializeField]
    private float mutation_rate = 0.05F;

    protected override void Awake()
    {
        base.Awake();
        _trees_population = new Dictionary<BiomType, List<Tree>>()
        {
            { BiomType.DESERT, new List<Tree>() },
            { BiomType.MOUNTAIN, new List<Tree>() },
            { BiomType.PLAIN, new List<Tree>() },
            { BiomType.SWAMP, new List<Tree>() },
        };
    }

    private void Start()
    {
        for(uint i = 0; i < _start_population_size; i++)
        {
            // SPAWN NEW TREE IN SCENE
            Vector3 tree_position = TerrainManager.Instance.get_random_position();

            Tree new_tree = Instantiate(
                _tree_prefab,
                tree_position,
                Quaternion.identity,
                _trees_container
            ).GetComponent<Tree>();

            new_tree.init();

            // REGISTER NEW TREE
            BiomType tree_biom =EnvironmentManager.Instance.get_biom(tree_position);
            _trees_population[tree_biom].Add(new_tree);
        }

        next_generation();
    }

    // compute the next generation
    private void next_generation()
    {
        selection();
        reproduction();
        StartCoroutine(grow_trees());
    }

    //This is a coroutine that will grow the trees, every 0.05 seconds a tree will grow
    //When all the trees have grown, the next generation will be computed
    IEnumerator grow_trees()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05F);

        foreach (KeyValuePair<BiomType, List<Tree>> biom in _trees_population)
        {
            foreach (Tree tree in _trees_population[biom.Key])
            {
                tree.grow();
                yield return wait;
            }
        }
        Invoke("next_generation", 0.5F);
    }

    //This function will select the best trees and kill the worst ones
    private void selection()
    {
        foreach (KeyValuePair<BiomType, List<Tree>> biom in _trees_population)
        {
            _trees_population[biom.Key].Sort(
                delegate (Tree a, Tree b)
                {
                    if (a.fitness_score < b.fitness_score)
                    {
                        return 1;
                    }
                    else if (a.fitness_score > b.fitness_score)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            );

            int amonth_to_kill = _trees_population[biom.Key].Count / 2;

            if ((_trees_population[biom.Key].Count - amonth_to_kill) % 2 != 0) amonth_to_kill--;

            for (int i = _trees_population[biom.Key].Count - amonth_to_kill; i < _trees_population[biom.Key].Count; i++)
            {
                _trees_population[biom.Key][i].transform.Destroy();
            }

            _trees_population[biom.Key].RemoveRange(_trees_population[biom.Key].Count - amonth_to_kill, amonth_to_kill);
        }
    }


    //This function take trees from the same biom and make them reproduce
    private void reproduction()
    {
        Dictionary<BiomType, List<Tree>> childs = new Dictionary<BiomType, List<Tree>>()
        {
            { BiomType.DESERT, new List<Tree>() },
            { BiomType.MOUNTAIN, new List<Tree>() },
            { BiomType.PLAIN, new List<Tree>() },
            { BiomType.SWAMP, new List<Tree>() },
        };

        foreach (KeyValuePair<BiomType, List<Tree>> biom in _trees_population)
        {
            for (int i = 1; i < _trees_population[biom.Key].Count; i += 2)
            {
                Tree mother = _trees_population[biom.Key][i - 1];
                Tree father = _trees_population[biom.Key][i];

                Tree first_child_genotype = new Tree();
                Tree second_child_genotype = new Tree();

                // TRUNK CROSSOVER
                if (Random.Range(0, 100) > 50)
                {
                    first_child_genotype.trunk = mother.trunk;
                    second_child_genotype.trunk = father.trunk;
                }
                else
                {
                    first_child_genotype.trunk = father.trunk;
                    second_child_genotype.trunk = mother.trunk;
                }

                // BARK CROSSOVER
                if (Random.Range(0, 100) > 50)
                {
                    first_child_genotype.bark = mother.bark;
                    second_child_genotype.bark = father.bark;
                }
                else
                {
                    first_child_genotype.bark = father.bark;
                    second_child_genotype.bark = mother.bark;
                }

                // FOLIAGE SHAPE CROSSOVER
                if (Random.Range(0, 100) > 50)
                {
                    first_child_genotype.foliage_shape = mother.foliage_shape;
                    second_child_genotype.foliage_shape = father.foliage_shape;
                }
                else
                {
                    first_child_genotype.foliage_shape = father.foliage_shape;
                    second_child_genotype.foliage_shape = mother.foliage_shape;
                }

                // FOLIAGE COLOR CROSSOVER
                if (Random.Range(0, 100) > 50)
                {
                    first_child_genotype.foliage_color = mother.foliage_color;
                    second_child_genotype.foliage_color = father.foliage_color;
                }
                else
                {
                    first_child_genotype.foliage_color = father.foliage_color;
                    second_child_genotype.foliage_color = mother.foliage_color;
                }

                // MUTATION
                mutation(ref first_child_genotype);
                mutation(ref second_child_genotype);

                // FIRST CHILD
                Vector3 first_child_position = TerrainManager.Instance.get_random_position_around(mother.transform.position, 25.0F);
                Tree child_1 = Instantiate(
                    _tree_prefab,
                    first_child_position,
                    Quaternion.identity,
                    _trees_container
                ).GetComponent<Tree>();

                child_1.init(first_child_genotype);

                BiomType child_biom = EnvironmentManager.Instance.get_biom(first_child_position);
                childs[child_biom].Add(child_1);

                // SECOND CHILD
                Vector3 second_child_position = TerrainManager.Instance.get_random_position_around(father.transform.position, 25.0F);
                Tree child_2 = Instantiate(
                    _tree_prefab,
                    TerrainManager.Instance.get_random_position(),
                    Quaternion.identity,
                    _trees_container
                ).GetComponent<Tree>();

                child_2.init(second_child_genotype);


                child_biom = EnvironmentManager.Instance.get_biom(second_child_position);
                childs[child_biom].Add(child_2);
            }
        }

        foreach (KeyValuePair<BiomType, List<Tree>> biom in _trees_population)
        {
            _trees_population[biom.Key].AddRange(childs[biom.Key]);
        }
    }

    private void mutation(ref Tree tree)
    {
        if (Random.Range(0f, 1f) < mutation_rate)
        {
            tree.trunk = get_random_trunk();
        }

        if (Random.Range(0f, 1f) < mutation_rate)
        {
            tree.bark = get_random_bark();
        }

        if (Random.Range(0f, 1f) < mutation_rate)
        {
            tree.foliage_shape = get_random_foliage_shape();
        }

        if (Random.Range(0f, 1f) < mutation_rate)
        {
            tree.foliage_color = get_random_foliage_color();
        }
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
