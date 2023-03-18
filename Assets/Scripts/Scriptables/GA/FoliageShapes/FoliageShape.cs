using UnityEngine;
using UnityEngine.Events;

public abstract class FoliageShape : Attribut
{
    [SerializeField]
    public GameObject foliage_prefab;

    public virtual Quaternion get_orientation(Quaternion turtle)
    {
        return turtle;
    }
}
