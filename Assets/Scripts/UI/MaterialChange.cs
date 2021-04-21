using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialChange : MonoBehaviour
{
    public SkinnedMeshRenderer target;
    public Material[] materials;
    private int iter = 0;
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(MatCycle);
    }

    // Update is called once per frame
    void MatCycle()
    {
        target.material = materials[iter];
        iter++;
        if (iter >= materials.Length)
        {
            iter = 0;
        }
    }
}
