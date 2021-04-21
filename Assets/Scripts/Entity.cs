using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is the base class for the player and the enemy
public class Entity : MonoBehaviour
{
    [Header(header: "Hit Points")]
    public int maxHealth = 100;

    [Header(header: "Weak Points")]
    public string weakpointName;
    public float weakpointMod = 1;

    // Health Points
    protected float m_hp;
    protected Ragdoll m_ragdoll;
    protected GameManager m_gameManager;

    protected virtual void Start()
    {
        // Find and Set GameManager
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // Set Ragdoll
        m_ragdoll = GetComponentInChildren<Ragdoll>();
        // Put HitDetector on every part of the rigidbody
        if (m_ragdoll.rigidbodies != null && m_ragdoll.rigidbodies.Count > 0)
        {
            foreach (Rigidbody rb in m_ragdoll.rigidbodies)
            {
                rb.gameObject.AddComponent<HitDetector>();
            }
        }
        // If there is no Ragdoll then put a HitDetector on the gameObject directly
        else
        {
            gameObject.AddComponent<HitDetector>();
        }
        m_hp = maxHealth;
        // Turn off the Ragdoll (set ragdoll parts as kinamatic)
        RagdollState(false);
    }
    // Base function for taking damage
    public virtual void TakeDamage(float damage)
    {
        m_hp -= damage;
        // When the entity has run out of Health Points, trigger Death
        if (m_hp <= 0)
        {
            Die();
        }
    }
    // Base function for ending the entity
    virtual protected void Die()
    {
        m_ragdoll.RagdollOn = true;
    }
    // Base function for restarting entity
    virtual public void Respawn()
    {
        m_hp = maxHealth;
        m_ragdoll.RagdollOn = false;
    }
    // Base function to set the entity's active state
    virtual public void SetActive(bool toggle)
    {
        enabled = toggle;
        RagdollState(!toggle);
    }
    // Base function to set the entity's ragdoll state
    virtual public void RagdollState(bool toggle)
    {

    }
}
