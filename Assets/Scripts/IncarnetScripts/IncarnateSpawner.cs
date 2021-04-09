using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncarnateSpawner : MonoBehaviour
{
    public bool randomizeLevels;
    public int level;
    public Vector2Int levelBounds;
    //int minLevel { get { return levelBounds.x; } }
    //int maxLevel { get { return levelBounds.y; } }
    public bool randomizeNumber;
    public int spawnNumber;
    public Vector2Int spawnNumberBounds;
    //int minSpawnNumber;
    //int maxSpawnNumber;
    public bool randomizeSize;
    //public float spawnSize;
    //public Vector2 spawnSizeBounds;
    
    //Make a weight system
    public GameObject[] spawnList;
    
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }
    void Spawn()
    {
        if (randomizeNumber)
        {
            spawnNumber = Random.Range(spawnNumberBounds.x, spawnNumberBounds.y);
        }
        for (int i = 0; i < spawnNumber; i++)
        {
            GameObject Incarnate = Instantiate(spawnList[Random.Range(0, spawnList.Length)]);
            Incarnate.transform.position = transform.position;
            if (randomizeLevels)
            {
                level = Random.Range(levelBounds.x, levelBounds.y);
            }
            if (randomizeSize)
            {
                Incarnate.GetComponent<IncarnateData>().randomSize = randomizeSize;
            }
            Incarnate.GetComponent<IncarnateData>().level = level;
            //Incarnate.transform.localScale = new Vector3(spawnSize, spawnSize, spawnSize);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
