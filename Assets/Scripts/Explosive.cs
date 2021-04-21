using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    // One important thing to note is that the radius only effects the hitbox
    // Not the particle effects
    [Header(header:"Stats")]
    public int m_damage;
    public float m_radius;
    public float m_detonationTime;
    [Tooltip("Time for explosive to play wind-up effects")]
    public float m_windUpTime;
    [Tooltip("Time the hit-box is avtive for")]
    public float m_activeExplosionTime;
    [Tooltip("Time for explosive to finish effects")]
    public float m_windDownTime;
    [Header("Particle Effect")]
    public ParticleSystem m_explosionParticles;

    private AudioSource m_explosionSFX;
    private ParticleSystem m_attachedParticles;
    private bool m_hasExploded = false;
    private MeshRenderer m_meshRenderer;
    private SphereCollider m_grenadeHitBox;
    private Rigidbody m_grenadeBody;

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        m_grenadeHitBox = GetComponent<SphereCollider>();
        m_grenadeBody = GetComponent<Rigidbody>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_explosionSFX = GetComponent<AudioSource>();
        // Add particles to the grenade to play
        m_attachedParticles = Instantiate(m_explosionParticles, transform);
    }

    // Update is called once per frame
    void Update()
    {
        // Count down.
        // when it hits zero, run Explode Coroutine
        m_detonationTime -= Time.deltaTime;
        if (m_detonationTime <= m_windUpTime && !m_hasExploded)
        {
            //Explode
            StartCoroutine(Explode());
        }
    }
    // Trigger the objects explosion
    IEnumerator Explode()
    {
        // This part is for anything that should occur just before the bomb explodes
        // Like playing sounds and setting the bomb's exploded state to true;
        m_hasExploded = true;
        m_explosionSFX.Play();
        yield return new WaitForSeconds(m_windUpTime);

        // This is where the explosion occurs
        // grenade mesh is disabled, hit box and particles are enabled
        m_grenadeHitBox.isTrigger = true;
        m_grenadeHitBox.radius = m_radius;
        m_grenadeBody.isKinematic = true;
        m_meshRenderer.enabled = false;
        m_attachedParticles.Emit(1);
        
        yield return new WaitForSeconds(m_activeExplosionTime);
        // disable the hit box and wind, explosion is winding down
        m_grenadeHitBox.enabled = false;

        yield return new WaitForSeconds(m_windDownTime);
        // Explosion is over
        Destroy(gameObject);
    }
    // If an enemy is in the hitbox and has a HitDetector component, deal damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HitDetector>() != null)
        {
            other.GetComponent<HitDetector>().Hit(m_damage);
        }
    }
}