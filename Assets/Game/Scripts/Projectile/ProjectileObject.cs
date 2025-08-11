using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    [Header("Projectile FX")]
    [SerializeField] GameObject FlyFx;
    [SerializeField] ParticleSystem HitFx;
    [SerializeField] GameObject meshHolder;
    [SerializeField] TrailRenderer trailRenderer;
    [Header("Ricochet")]
    [SerializeField] int ricochetLimit = 1;
    [Header("Penetration")]
    [SerializeField] bool penetrationActive;
    [SerializeField] int penetrationLimit;
    private int penetrationCount;
    [Header("Destroy Time")]
    [SerializeField] float destroyTime = 1f;
    WaitForSeconds deactivateWaitTime;
    private Rigidbody projectileRigidbody;
    private Collider projectileCollider;
    private float projectileSpeed = 70f;
    private float projectileDamage;
    private float ricochetCount;
    private bool isProjectileHit;
    private bool isBullet = true;

    private void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
        projectileCollider = GetComponent<Collider>();
        deactivateWaitTime = new WaitForSeconds(destroyTime);
        if (trailRenderer == null)
            trailRenderer = GetComponentInChildren<TrailRenderer>();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        isProjectileHit = false;
        ricochetCount = 0;
        penetrationCount = 0;
        meshHolder.SetActive(true);
        ResetObject();
        ResetTrail();
    }

    public void SetProjectile(float speed, float damage, bool isBullet = true)
    {
        projectileSpeed = speed;
        projectileDamage = damage;
        this.isBullet = isBullet;
        StartCoroutine(WaitForDeactivate(deactivateWaitTime));
    }
    private void FixedUpdate()
    {
        if (!isProjectileHit)
        {
            projectileRigidbody.velocity = transform.forward * projectileSpeed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.gameObject);
    }
    private void HandleHit(GameObject hitObject)
    {
        if (!gameObject.activeSelf || isProjectileHit) return;

        if (hitObject.CompareTag("Enemy"))
        {
            EnemyController enemy = hitObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(projectileDamage);
            }

            DeactivateProjectile();
        }
        ProcessFinalHit();
    }
    private void ProcessFinalHit()
    {
        isProjectileHit = true;
        projectileRigidbody.velocity = Vector3.zero;
        ShowHitFX();
        meshHolder.SetActive(false);
        StartCoroutine(WaitForDeactivate(new WaitForSeconds(1f)));
    }

    private void ResetObject()
    {
        transform.localScale = Vector3.one;
        projectileRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        projectileRigidbody.velocity = Vector3.zero;
        projectileCollider.enabled = true;
    }

    private void DeactivateProjectile()
    {
        ProcessFinalHit();
    }
    private IEnumerator WaitForDeactivate(WaitForSeconds waitTime)
    {
        yield return waitTime;
        gameObject.SetActive(false);
    }
    private void ResetTrail()
    {
        if (trailRenderer != null)
        {
            trailRenderer.Clear();
        }
    }
    private void ShowHitFX()
    {
        if (HitFx != null)
        {
            if (HitFx.isPlaying)
            {
                HitFx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            HitFx.Play();
        }
    }
}
