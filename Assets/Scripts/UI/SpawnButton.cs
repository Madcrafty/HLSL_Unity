using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    public GameObject thing;
    public Material[] materials;
    //void Awake()
    //{
    //    GetComponent<Button>().onClick.AddListener(Spawn);
    //}
    public void Spawn()
    {
        GameObject tmp = Instantiate(thing, transform.position, new Quaternion(0,0,0,1));
        tmp.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length - 1)];
        tmp.GetComponent<Rigidbody>().AddForce(transform.forward*100, ForceMode.VelocityChange);
    }
}
