using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Used to detect if an Entity is hit
public class HitDetector : MonoBehaviour
{
    private Entity entity = null;
    private float m_damageMod = 1;

    private void Start()
    {
        // Get entity componenet
        entity = GetComponentInParent<Entity>();
        if (entity.weakpointName == name)
        {
            m_damageMod = entity.weakpointMod;
        }
    }
    // Causes entity to take damage
    public void Hit(int baseDamage)
    {
        // Calculate damage taken, then tell the entity to take that damage
        float totalDamage = baseDamage * m_damageMod;
        entity.TakeDamage(totalDamage);
    }
}
