using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public int damage = 20;
    public float shotCost = 5.0f;
    public GameObject projectile = null;
    public float projCost = 50.0f;
    public float projSpeed = 50.0f;
    public float effectTime = 0.1f;
    public float maxCapacity = 100;
    public float rechargeRate = 1;

    private float capacity;
    private float elapsedLaserTime = 0;
    private Vector3 targetPos;
    private Vector3 originPoint;
    private Collider barrel;
    private LineRenderer laser;
    private Slider chargeMeter;
    private float elapsedShootTime = 0;
    private ParticleSystem smoke;
    private ParticleSystem flash;
    private AudioSource soundSource;
    // Start is called before the first frame update
    void Start()
    {
        // Get components
        soundSource = GetComponent<AudioSource>();
        smoke = transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
        flash = transform.GetChild(2).GetChild(1).GetComponent<ParticleSystem>();
        chargeMeter = transform.GetChild(3).GetChild(0).GetComponent<Slider>();
        laser = GetComponent<LineRenderer>();
        barrel = transform.GetChild(2).GetComponent<Collider>();
        // Set Variables
        capacity = maxCapacity;
        originPoint = flash.transform.position;
        // ... and laser
        laser.SetPosition(0, originPoint);
    }

    // Update is called once per frame
    void Update()
    {
        #region Charge Meter
        // When the gun isn't at full charge, recharge the gun
        // Recharges faster the longer you stop shooting
        if (capacity < maxCapacity)
        {
            elapsedShootTime += Time.deltaTime;
            capacity += rechargeRate * elapsedShootTime * Time.deltaTime;
        }
        // If Charge is over max, dont be 
        if (capacity > maxCapacity)
        {
            capacity = maxCapacity;
        }
        // Update the charge metter
        chargeMeter.value = capacity / maxCapacity;
        #endregion

        #region Shooting
        // Left click to shoot a lazer, if you have the energy
        if (Input.GetMouseButtonDown(0) && capacity >= shotCost)
        {
            ShootLaser();
        }
        // Left click to shoot a projectile, if you have the energy
        if (Input.GetMouseButtonDown(1) && capacity >= projCost)
        {
            ShootProjectile();
        }
        // Display laser for a short time when firing
        if (laser.enabled)
        {
            elapsedLaserTime += Time.deltaTime;
            if (elapsedLaserTime >= effectTime)
            {
                laser.enabled = false;
            }
        }
        // Reset timer afterwards
        else if (elapsedLaserTime > 0)
        {
            elapsedLaserTime = 0;
        }
        #endregion
        // Smoke effects
        if (capacity <= 75 && smoke.isPlaying == false)
        {
            smoke.Play();
        }
        if (capacity >= 75 && smoke.isPlaying == true)
        {
            smoke.Stop();
        }
    }
    private void FixedUpdate()
    {
        // Update location of the shoot point
        originPoint = flash.transform.position;
        laser.SetPosition(0, originPoint);
    }
    // Shoot a laser
    public void ShootLaser()
    {
        // Create a ray from centre screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Make raycaset
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, 500);

        // if it hits nothing, set target posiiton to max distance
        if (hitInfo.point == new Vector3(0,0,0))
        {
            targetPos = (Camera.main.transform.forward * 500) + transform.position;
        }
        // otherwise return the point hit
        else
        {
            targetPos = hitInfo.point;
            // if the thing hit has a HitDetector, deal damage
            if (hitInfo.transform.GetComponent<HitDetector>() != null)
            {
                hitInfo.transform.GetComponent<HitDetector>().Hit(damage);
            }
        }
        // Set laser end position to the target
        laser.SetPosition(1, targetPos);
        laser.enabled = true;
        // Special effects
        flash.Emit(1);
        soundSource.Play();
        // Remove cost from capacity
        capacity -= shotCost;
        elapsedShootTime = 0;
    }
    // Shoot a Projectile
    public void ShootProjectile()
    {
        // Instatniate the prefab
        GameObject tmp = Instantiate(projectile);
        tmp.transform.position = originPoint;
        tmp.GetComponent<Rigidbody>().AddForce(transform.forward * projSpeed, ForceMode.VelocityChange);
        // Remove cost from capacity
        capacity -= projCost;
        elapsedShootTime = 0;
    }
}
