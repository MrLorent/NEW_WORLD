using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private float colorTransitionTime = 5.0f;
    
    Material environmentMaterial;

    

    // Start is called before the first frame update
    void Start()
    {
        environmentMaterial = gameObject.GetComponent<Renderer>().material;
        StartCoroutine("CycleColors");
    }

    IEnumerator CycleColors()
    {
        Vector3 previousColor = new Vector3(environmentMaterial.color.r, environmentMaterial.color.g, environmentMaterial.color.b);
        Vector3 currentColor = previousColor;

        while(true)
        {
            Vector3 newColor = new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
            Vector3 deltaColor = (newColor - previousColor) * (1.0f / colorTransitionTime);

            //We constantly moving from our actual color to our newColor, when we are close enough
            //We put the newColor as the previousColor and start again
            //This is calculated each frame
            while((newColor - currentColor).magnitude > 0.1f)
            {
                currentColor = currentColor + deltaColor * Time.deltaTime;
                environmentMaterial.color = new Color(currentColor.x, currentColor.y, currentColor.z);

                yield return null;
            }

            previousColor = newColor;
        }
    }

    
}
