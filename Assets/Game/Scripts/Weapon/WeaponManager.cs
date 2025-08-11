using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private FindTarget findTargetSystem;

    [Header("Available Weapons")]
    [SerializeField] private List<WeaponDataSO> weaponDataList;
    [Header("Muzzle")]
    [SerializeField] private ParticleSystem muzzleFlash;

    private float nextFireTime;
    private float baseDamage;
    private float activeFireRate;

    private IWeapon currentWeapon;
    private WeaponDataSO currentWeaponData;

    private void Awake()
    {
        if (weaponDataList.Count > 0)
        {
            EquipWeapon(weaponDataList[0]); //Equip first weapon by default.
        }
    }

    public void EquipWeapon(WeaponDataSO weaponData)
    {
        currentWeaponData = weaponData;
        currentWeapon = weaponData.GetWeaponInstance();
        baseDamage = weaponData.baseDamage;
        activeFireRate = GetTotalFireRate();
    }

    private void Update()
    {
        if (findTargetSystem == null) return;

        Transform closestEnemy = findTargetSystem.GetClosestEnemy();
        if (closestEnemy == null) return;

        Vector3 adjustedEnemyPosition = new Vector3(closestEnemy.position.x, transform.position.y, closestEnemy.position.z);
        Vector3 directionToEnemy = adjustedEnemyPosition - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, directionToEnemy);

        if (Time.time >= nextFireTime && currentWeapon.CanShoot(angleToTarget))
        {
            currentWeapon.Shoot(shootOrigin, GetTotalDamage());
            nextFireTime = Time.time + activeFireRate;
            //Show muzzle.
            ShowMuzzleFlash();
        }
    }

    public float GetTotalDamage()
    {
        return baseDamage;
    }

    public float GetTotalFireRate()
    {
        return currentWeapon.GetFireRate();
    }

    private void ShowMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            if (muzzleFlash.isPlaying)
            {
                muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            muzzleFlash.Play();
        }
    }
}
