using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public GameManager gm;
    [Header("Spawn Function")]
    public GameObject thing;
    public ParticleSystem juice;

    [Header("HoloBridge Function")]
    public GameObject[] bridge;
    public Material onMat;
    public Material offMat;
    public ParticleSystem effect;

    
    void Awake()
    {
        //GetComponent<Button>().onClick.AddListener(Spawn);
        //gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void Spawn()
    {
        thing.SetActive(!thing.activeInHierarchy);
        juice.Play();
        StartCoroutine(gm.BlurScreenForTime(5, 1));
        gm.SetTimerState(true);
    }
    // It is used toggle the HollowBridges, but can be used to toggle other objects too
    public void HollowBridge()
    {
        foreach (GameObject comp in bridge)
        {
            bool enabled = comp.GetComponent<Collider>().enabled;
            comp.GetComponent<Collider>().enabled = !enabled;
            if (!enabled)
            {
                comp.GetComponent<Renderer>().material = onMat;
            }
            else
            {
                comp.GetComponent<Renderer>().material = offMat;
            }
            effect.Play();
        }
    }
}
