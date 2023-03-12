using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Genetic Algorim/Attributs Index")]
public class AttributsIndex : ScriptableObject
{
    public List<Trunk> trunks;
    public List<Bark> barks;
    public List<FoliageShape> foliage_shapes;
    public List<FoliageColor> foliage_colors;
}
