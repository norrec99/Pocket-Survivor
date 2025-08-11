using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RifleData", menuName = "Weapons/Rifle")]
public class RifleDataSO : WeaponDataSO
{
    public string projectileTag = "RifleProjectile";
    public override IWeapon GetWeaponInstance()
    {
        return new Rifle(this);
    }

    private class Rifle : IWeapon
    {
        private RifleDataSO weaponData;

        public Rifle(RifleDataSO data)
        {
            weaponData = data;
        }

        public bool CanShoot(float angleToTarget) => angleToTarget <= weaponData.precisionAngle;

        public void Shoot(Transform shootOrigin, float weaponDamage)
        {
            FireBullet(shootOrigin, weaponDamage);
        }

        private void FireBullet(Transform shootOrigin, float damage)
        {
            GameObject bullet = ObjectPooler.Instance.GetPooledObject(
                weaponData.projectileTag,
                shootOrigin.position,
                shootOrigin.rotation
            );

            if (bullet != null)
            {
                bullet.GetComponent<ProjectileObject>().SetProjectile(
                    weaponData.projectileSpeed,
                    damage
                );
            }
        }
        public float GetFireRate() => weaponData.fireRate;
    }
}
