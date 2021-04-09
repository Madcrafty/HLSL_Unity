using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public GameObject thing;
    private GameManager gm;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Spawn);
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Spawn()
    {
        thing.SetActive(!thing.activeInHierarchy);
        StartCoroutine(gm.BlurScreenForTime(5, 1));
    }
}
