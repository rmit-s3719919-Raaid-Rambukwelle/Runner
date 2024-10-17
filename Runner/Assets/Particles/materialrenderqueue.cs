using UnityEngine;

[ExecuteInEditMode]
public class ForceRenderQueue : MonoBehaviour
{
    public Material particleMaterial;
    public int renderQueue = 3100; // Set to your desired value

    void Start()
    {
        if (particleMaterial != null)
        {
            particleMaterial.renderQueue = renderQueue;
        }
    }

    void Update()
    {
        if (particleMaterial != null && particleMaterial.renderQueue != renderQueue)
        {
            particleMaterial.renderQueue = renderQueue;
        }
    }
}