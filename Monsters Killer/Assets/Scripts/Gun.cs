using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations.Rigging;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform cam;
    public float fireRate;
    public TwoBoneIKConstraint rightHandConstraint, leftHandConstraint;
    [SerializeField] LayerMask layers;
    public float recoilRotationX, recoilRotationY, recoilRotationZ;
    [SerializeField] float recoilPositionX, recoilPositionY, recoilPositionZ;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] Transform barel;
    [SerializeField] float damage;
    [SerializeField] int maxMag;
    public int mag;
    [SerializeField] int maxAmmo;
    public int ammo;
    [SerializeField] string reloadWeaponDir;
    [SerializeField] TextMeshProUGUI weaponAmmoTextNormal , weaponAmmoTextAlert, weaponMagText;


    Animator anim;
    public bool isReloading = false;
    public bool canShoot = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetUpWeapon()
    {
        mag = maxMag;
        ammo = maxAmmo;
    }

    private void Update()
    {
        canShoot = !isReloading && ammo != 0;

        weaponAmmoTextNormal.gameObject.SetActive(!(ammo <= maxAmmo / 4));
        weaponAmmoTextAlert.gameObject.SetActive(ammo <= maxAmmo / 4);

        weaponAmmoTextNormal.text = ammo + "";
        weaponAmmoTextAlert.text = ammo + "";

        weaponMagText.text = mag + "";
        
    }

    public void Shot(GameObject bullet, bool isAmining)
    {
        if (ammo == 0)
            Reload();

        ammo--;
        Bullet b = Instantiate(bullet, barel.position, Quaternion.identity).GetComponent<Bullet>();

        RaycastHit hitPoint;
        Ray ray = new Ray(cam.position , cam.forward);

        if (Physics.Raycast(ray , out hitPoint , 100f , layers))
        {
            b.transform.forward = hitPoint.point - barel.position;
        }

        b.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        muzzleFlash.Play();
        GetComponent<Recoil>().FireRecoilRotation((isAmining ? 0.5f : recoilRotationX) ,
            (isAmining ? 0.5f : recoilRotationY), (isAmining ? 0.5f : recoilRotationZ));
        GetComponent<Recoil>().FireRecoilPosition((isAmining ? 0.001f : recoilPositionX),
            (isAmining ? 0.001f : recoilPositionY), (isAmining ? 0.001f : recoilPositionZ));

        if (ammo == 0)
        {
            Reload();
        }
    }

    public void Reload()
    {
        if (mag == 0 || ammo == maxAmmo || isReloading)
            return;

        transform.parent.GetComponent<WeponsHolder>().StartAnimation(reloadWeaponDir);
        anim.SetTrigger("Reload");
        isReloading = true;
    }

    public void Reloaded()
    {
        isReloading = false;
        ammo = maxAmmo;
        mag -= maxAmmo;
        if (mag < 0)
        {
            ammo += mag;
            mag = 0;
        }
    }
}
