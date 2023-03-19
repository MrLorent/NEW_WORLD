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

        next_generation();
    }

    private void next_generation()
    {
        Debug.Log("Nb trees in list : " + _trees_population.Count);
        selection(ref _trees_population);
        reproduction(ref _trees_population);
        StartCoroutine(grow_trees());
    }

    IEnumerator grow_trees()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05F);
        foreach (Tree tree in _trees_population)
        {
            tree.grow();
            yield return wait;
        }
        Invoke("next_generation", 0.5F);
    }

    private void selection(ref List<Tree> _biom_population)
    {
        _biom_population.Sort(
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

        int amonth_to_kill = _trees_population.Count / 2;

        if ((_trees_population.Count - amonth_to_kill) % 2 != 0) amonth_to_kill--;
        
        for(int i = _biom_population.Count - amonth_to_kill; i < _biom_population.Count; i++)
        {
            _trees_population[i].transform.Destroy();
        }

        _biom_population.RemoveRange(_biom_population.Count - amonth_to_kill, amonth_to_kill);
    }

    private void reproduction(ref List<Tree> _biom_population)
    {
        List<Tree> childs = new List<Tree>();

        for(int i = 1;  i < _biom_population.Count; i += 2)
        {
            Tree mother = _biom_population[i - 1];
            Tree father = _biom_population[i];

            Tree first_child_genotype = new Tree();
            Tree second_child_genotype = new Tree();

            // TRUNK CROSSOVER
            if(Random.Range(0, 100) > 50)
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

            mutation(ref first_child_genotype);
            mutation(ref second_child_genotype);

            Tree child_1 = Instantiate(
                _tree_prefab,
                TerrainManager.Instance.get_random_position(),
                Quaternion.identity,
                _trees_container
            ).GetComponent<Tree>();

            Tree child_2 = Instantiate(
                _tree_prefab,
                TerrainManager.Instance.get_random_position(),
                Quaternion.identity,
                _trees_container
            ).GetComponent<Tree>();

            child_1.init(first_child_genotype);
            child_2.init(second_child_genotype);

            childs.Add(child_1);
            childs.Add(child_2);
        }

        _biom_population.AddRange(childs);
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
