using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class LineRendererEditor : MonoBehaviour
{
    private LineRenderer lr;
    private Transform anchorPoint;
    // Start is called before the first frame update
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        anchorPoint = transform.GetChild(2).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, anchorPoint.position);
        lr.SetPosition(1, anchorPoint.position);
    }
}
