using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    /*====== PUBLIC ======*/
    [Header("PEFABS")]
    public GameObject CylinderPrefab;

    [Header("TREE PARAMETERS")]
    public int number_of_iteration = 5;

    /*====== PRIVATE ======*/
    private List<GameObject> tree;

    // Start is called before the first frame update
    void Start()
    {
        BuildTree(number_of_iteration, new Vector3(0,1,0));   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildTree(int nb_iteration, Vector3 position) {
        // END CONDITION
        if(nb_iteration == 0) return;

        GameObject new_step = Instantiate(CylinderPrefab, position, Quaternion.identity);
        
        // NEXT STEP
        position += new Vector3(0, 2, 0);
        nb_iteration--;
        BuildTree(nb_iteration, position);
    }
}
