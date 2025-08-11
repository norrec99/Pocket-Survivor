using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponDataSO : ScriptableObject
{
    public float projectileSpeed = 70f;
    public float baseDamage = 25f;
    public float fireRate = 0.5f;
    public float precisionAngle = 5f;
    public int magazineSize = 30;
    public float magazineSwitchTime = 2f;
    public abstract IWeapon GetWeaponInstance();
}
