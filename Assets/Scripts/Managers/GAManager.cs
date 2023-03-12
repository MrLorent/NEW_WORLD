using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAManager : Singleton<GAManager>
{
    [SerializeField]
    private AttributsIndex attributs_index;

    public Trunk get_random_trunk()
    {
        return attributs_index.trunks[Random.Range(0, attributs_index.trunks.Count)];
    }

    public Bark get_random_bark()
    {
        return attributs_index.barks[Random.Range(0, attributs_index.barks.Count)];
    }
}
