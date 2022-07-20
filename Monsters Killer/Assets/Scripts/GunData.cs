using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData" , menuName = "GunData")]
public class GunData : ScriptableObject
{
    public int weaponId;

    public float fireRate;
    public float damage;
    public int maxMag;
    public int maxAmmo;
    public int mag;
    public int ammo;


    public float recoilRotationX, recoilRotationY, recoilRotationZ;
    public float recoilPositionX, recoilPositionY, recoilPositionZ;
    public float aimRecoil;
    public float recoil;
    public string reloadWeaponDir;

    public GameObject impactEffect;
    public LayerMask layers;
}
