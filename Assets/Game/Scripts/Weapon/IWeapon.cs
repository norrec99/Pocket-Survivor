using UnityEngine;

public interface IWeapon
{
    void Shoot(Transform shootOrigin, float damage);
    bool CanShoot(float angleToTarget);
    float GetFireRate();
}
