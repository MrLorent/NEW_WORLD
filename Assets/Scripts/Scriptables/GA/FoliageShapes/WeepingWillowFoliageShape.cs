using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Genetic Algorim/Foliage Shape/Weeping Willow Foliage Shape")]
public class WeepingWillowFoliageShape : FoliageShape
{
    public override Quaternion get_orientation(Quaternion turtle)
    {
        return Quaternion.identity;
    }
}
