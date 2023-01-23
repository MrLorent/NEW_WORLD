using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    [SerializeField] private int populationSize = 10;
    [SerializeField] private float timeBetweenGeneration = .5f;
    [SerializeField] private float rateOfMutation = .1f;
    [SerializeField] private GameObject environment;
    [SerializeField] private GameObject Prefab;


    protected List<Tree> population = new List<Tree>();

    // Start is called before the first frame update
    void Start()
    {
        Bounds boundaries = environment.GetComponent<Renderer>().bounds;


        for(int i = 0; i < populationSize; i++)
        {
            Tree tree = CreateTree(boundaries);
            population.Add(tree);
        }
        StartCoroutine(EvaluationLoop());
    }

    //We create a Tree in a random position
    public Tree CreateTree(Bounds bounds)
    {
        Vector3 randomPosition = new Vector3(
            UnityEngine.Random.Range(-0.5f, 0.5f) * bounds.size.x,
            UnityEngine.Random.Range(-0.5f, 0.5f) * bounds.size.y,
            UnityEngine.Random.Range(-0.5f, 0.5f) * bounds.size.z
        );

        //We are using the world position so we can move our environment without breaking the behavior
        Vector3 worldPosition = environment.transform.position + randomPosition;
        

        //We put the tree on the plane rather than half in it
        //float height = temp.GetComponent<MeshFilter>().mesh.bounds.size.y;
        //worldPosition.y += 10 / 2.0f; 

        GameObject temp = Instantiate(Prefab, worldPosition, Quaternion.identity, transform);
        Tree tree = temp.AddComponent<Tree>();
        AssignRandomColor(temp);


        return tree;
    }

    public void AssignRandomColor(GameObject tree)
    {
        tree.GetComponent<Tree>().SetColor(
            new Color(
                UnityEngine.Random.Range(0.0f, 1.0f),
                UnityEngine.Random.Range(0.0f, 1.0f),
                UnityEngine.Random.Range(0.0f, 1.0f)
            )
        );

    }

    //This function evaluate the fitness by looking at the magnitude of the difference between our color
    float EvaluateFitness(Color environment, Color tree)
    {
        float fitness = (
                new Vector3(environment.r, environment.g, environment.b) -
                new Vector3(tree.r, tree.g, tree.b)).magnitude;
        return fitness;
    }

    //This function evaluate the Mutation that are applied to each of the babies
    //The rate of the mutation that happen is directed by rateOfMutation
    public Color EvaluateMutation(Color tree)
    {
        Vector3 mutatedColor = new Vector3(
            tree.r,
            tree.g,
            tree.b
        );

        for(int i = 0; i < 3; i++)
        {
            if(UnityEngine.Random.Range(0.0f, 1.0f) <= rateOfMutation)
            {
                mutatedColor[i] = UnityEngine.Random.Range(0.0f, 1.0f);
            }
        }
        return new Color(mutatedColor.x, mutatedColor.y, mutatedColor.z);
    }

    //This function breed some new babies
    void Breed()
    {
        //New Babies list
        List<Tree> tempList = new List<Tree>();

        for(int i = 1; i < population.Count; i+=2)
        {
            int mother = i-1;
            int father = i;

            float howGenesAreSplit = UnityEngine.Random.Range(0.0f, 1.0f);

            Bounds bounds = environment.GetComponent<Renderer>().bounds;
            Tree childTree1 = CreateTree(bounds);
            Tree childTree2 = CreateTree(bounds);

            tempList.Add(childTree1);
            tempList.Add(childTree2);

            Color tempColor;

            //Split up the genes based on random probability
            //We have 6 values and 100/6= 16.6 which is close enough
            if(howGenesAreSplit <= 0.16f)
            {
                tempColor = new Color(
                    population[mother].color.r,
                    population[mother].color.g,
                    population[father].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree1.SetColor(tempColor);

                tempColor = new Color(
                    population[father].color.r,
                    population[father].color.g,
                    population[mother].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree2.SetColor(tempColor);

            }
            else if(howGenesAreSplit <= 0.32f)
            {
                tempColor = new Color(
                    population[mother].color.r,
                    population[father].color.g,
                    population[mother].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree1.SetColor(tempColor);

                tempColor = new Color(
                    population[father].color.r,
                    population[mother].color.g,
                    population[father].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree2.SetColor(tempColor);
            }
            else if(howGenesAreSplit <= 0.48f)
            {
                tempColor = new Color(
                    population[mother].color.r,
                    population[father].color.g,
                    population[father].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree1.SetColor(tempColor);

                tempColor = new Color(
                    population[father].color.r,
                    population[mother].color.g,
                    population[mother].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree2.SetColor(tempColor);
            }
            else if(howGenesAreSplit <= 0.64f)
            {
                tempColor = new Color(
                    population[father].color.r,
                    population[mother].color.g,
                    population[father].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree1.SetColor(tempColor);

                tempColor = new Color(
                    population[mother].color.r,
                    population[father].color.g,
                    population[mother].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree2.SetColor(tempColor);
            }
            else if(howGenesAreSplit <= 0.80f)
            {
                tempColor = new Color(
                    population[father].color.r,
                    population[father].color.g,
                    population[mother].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree1.SetColor(tempColor);

                tempColor = new Color(
                    population[mother].color.r,
                    population[mother].color.g,
                    population[father].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree2.SetColor(tempColor);
            }
            else
            {
                tempColor = new Color(
                    population[father].color.r,
                    population[mother].color.g,
                    population[father].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree1.SetColor(tempColor);

                tempColor = new Color(
                    population[mother].color.r,
                    population[father].color.g,
                    population[mother].color.b
                );
                tempColor = EvaluateMutation(tempColor);
                childTree2.SetColor(tempColor);
            }
        }
        //Concatenate lists together
        population.AddRange(tempList);
    }


    //This function evaluate the population which means it evaluate the fitness of each tree
    //Then it sort them by their fitness, kill half of them based on this sort and breed some new babies
    void EvaluatePopulation()
    {

        //Breed
        Breed();

        //Fitness
        for( int i = 0; i < population.Count; i++)
        {
            float fitness = EvaluateFitness(
                environment.GetComponent<MeshRenderer>().material.color,
                population[i].GetComponent<MeshRenderer>().material.color
            );
            population[i].fitnessScore = fitness;
        }

        //Sort from the best Fitness to the worst
        population.Sort( 
            delegate (Tree tree1, Tree tree2)
            {
                if(tree1.fitnessScore > tree2.fitnessScore)
                {
                    return 1;
                }
                else if( tree1.fitnessScore == tree2.fitnessScore)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
         );

        //kill some trees
        int halfwayMark = (int) population.Count / 2;

        if(halfwayMark % 2 != 0)
        {
            halfwayMark++;
        }

        for (int i = halfwayMark; i < population.Count; i++)
        {
            Destroy(population[i].gameObject);
            population[i] = null;
        }
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        population.RemoveRange(halfwayMark, population.Count - halfwayMark);

        
    }

    //We evaluate the tree color each 2secondes
    IEnumerator EvaluationLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(timeBetweenGeneration);
            EvaluatePopulation();
        }
    }
}
