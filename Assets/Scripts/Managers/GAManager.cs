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
    private uint _nb_trees = 10;

    [SerializeField]
    private Transform _tree_container;

    private List<Tree> _trees;

    protected override void Awake()
    {
        base.Awake();
        _trees = new List<Tree>();
    }

    private void Start()
    {
        for(uint i = 0; i < _nb_trees; i++)
        {
            Tree new_tree = Instantiate(
                _tree_prefab,
                TerrainManager.Instance.get_random_position(),
                Quaternion.identity,
                _tree_container
            ).GetComponent<Tree>();

            new_tree.init();

            _trees.Add(new_tree);
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
