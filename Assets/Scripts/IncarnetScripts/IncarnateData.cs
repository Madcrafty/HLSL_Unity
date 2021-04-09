using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncarnateData : MonoBehaviour
{
    [SerializeField]
    private int expThreshold;
    [SerializeField]
    private int expReqirementBoost = 0;
    [SerializeField]
    private int expBoost = 1;
    [SerializeField]
    private int experience;
    public int level;
    int friendship = 0;
    int[] currentStats;
    int[] baseStats;
    int[] IVs;
    int[] EVs;
    [SerializeField]
    private string ability;
    int critRate = 0;
    int fear;
    int loyalty;
    int courage;
    public bool randomSize;
    public float size = 1;
    public Vector2 sizeBounds;
    
    public float sightDistance = 5;
    
    public float targetedSightDistance = 15;

    //Natures
    //Emotional Stats
    //Battle Characteristics
    //Overworld Characteristics
    //Proficiencies

    //Model
    //MoveList
    //EvolutionRequirements

    // Start is called before the first frame update
    void Start()
    {
        if (sizeBounds == new Vector2(0,0))
        {
            sizeBounds.x = size * 0.6f;
            sizeBounds.y = size * 1.4f;
        }
        if (randomSize)
        {
            size = Random.Range(sizeBounds.x, sizeBounds.y);
        }
        transform.localScale = new Vector3(size, size, size);
        expThreshold = (level + expReqirementBoost) * (level + expReqirementBoost) + expReqirementBoost;
        int curExpMin = (level - 1 + expReqirementBoost) * (level - 1 + expReqirementBoost) + expReqirementBoost;
        experience = Random.Range(curExpMin, expThreshold - 1);
    }
    void AddExp(int exp)
    {
        experience += exp * expBoost;
        if (experience >= expThreshold)
        {
            LevelUp();
        }
    }
    void LevelUp()
    {
        level++;
        expThreshold = (level + expReqirementBoost) * (level + expReqirementBoost) + expReqirementBoost;
    }
    void AddFriendship(int _friendship)
    {
        friendship += _friendship;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
