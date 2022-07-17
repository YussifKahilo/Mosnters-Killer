using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations.Rigging;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject impactEffect;
    [SerializeField] TextMeshProUGUI weaponAmmoTextNormal , weaponAmmoTextAlert, weaponMagText;
    [SerializeField] Transform barel;
    [SerializeField] Transform cam;
    float cursorSize;
    [SerializeField] RectTransform cursor;
    [SerializeField] ParticleSystem muzzleFlash;
    public TwoBoneIKConstraint rightHandConstraint, leftHandConstraint;
    public float recoilRotationX, recoilRotationY, recoilRotationZ;
    [SerializeField] float recoilPositionX, recoilPositionY, recoilPositionZ;
    [SerializeField] string reloadWeaponDir;
    [SerializeField] LayerMask layers;
    public float fireRate;
    [SerializeField] float aimRecoil;
    [SerializeField] float recoil;
    [SerializeField] float damage;
    [SerializeField] int maxMag;
    public int mag;
    [SerializeField] int maxAmmo;
    public int ammo;

    bool isEnabled = false;
    Animator anim;
    public bool isReloading = false;
    public bool canShoot = true;

    float targetRecoilSize;
    [SerializeReference] float currentRecoilSize;

    float targetCursorSize;
    float currentCursorSize;

    float recoilAmount;
    [SerializeReference] PlayerMovement playerMovement;

    private void OnEnable()
    {
        isEnabled = true;
        if (ammo == 0)
        {
            Reload();
        }
    }

    private void OnDisable()
    {
        isEnabled = false;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        cursorSize = 4400 * recoil;
        targetCursorSize = cursorSize;
        currentCursorSize = cursorSize;
        targetRecoilSize = recoil;
        currentRecoilSize = recoil;
        recoilAmount = recoil;
    }

    public void SetUpWeapon()
    {
        mag = maxMag;
        ammo = maxAmmo;
    }

    private void Update()
    {
        recoilAmount = recoil + (playerMovement.movement.magnitude > 0 ? 0.05f : 0);
        

        canShoot = !isReloading;
        cursorSize = recoilAmount * 4400;

        weaponAmmoTextNormal.gameObject.SetActive(!(ammo <= maxAmmo / 4));
        weaponAmmoTextAlert.gameObject.SetActive(ammo <= maxAmmo / 4);

        weaponAmmoTextNormal.text = ammo + "";
        weaponAmmoTextAlert.text = ammo + "";

        weaponMagText.text = mag + "";
        
        if (isEnabled)
        {
            targetCursorSize = Mathf.Lerp(targetCursorSize , cursorSize , 6 * Time.deltaTime);
            currentCursorSize = Mathf.Lerp(currentCursorSize , targetCursorSize , 5 * Time.fixedDeltaTime);
            cursor.sizeDelta = new Vector2(currentCursorSize, currentCursorSize);

            targetRecoilSize = Mathf.Lerp(targetRecoilSize, recoilAmount, 6 * Time.deltaTime);
            currentRecoilSize = Mathf.Lerp(currentRecoilSize, targetRecoilSize, 5 * Time.fixedDeltaTime);
        }

    }

    void AddRecoil()
    {
        targetCursorSize += 400;
        targetRecoilSize += 0.025f;
    }

    public void Shot(GameObject bullet, bool isAmining)
    {
        if (ammo == 0)
            Reload();

        ammo--;
        Bullet b = Instantiate(bullet, barel.position, Quaternion.identity).GetComponent<Bullet>();


        Vector3 recoilAddVector = transform.parent.GetComponent<WeponsHolder>().isAiming ?
            new Vector3(Random.Range(-aimRecoil, aimRecoil), Random.Range(-aimRecoil, aimRecoil),
            Random.Range(-aimRecoil, aimRecoil)) :
            new Vector3(Random.Range(-currentRecoilSize, currentRecoilSize),
            Random.Range(-currentRecoilSize, currentRecoilSize), Random.Range(-currentRecoilSize, currentRecoilSize));

        RaycastHit hitPoint;
        Ray ray = new Ray(cam.position, cam.forward + recoilAddVector);
        Physics.Raycast(ray, out hitPoint, 100f, layers);

        ParticleSystem ie = Instantiate(impactEffect, hitPoint.point,
            Quaternion.LookRotation(hitPoint.normal)).GetComponent<ParticleSystem>();
        ie.Play();
        StartCoroutine(DestroyAfterTime(ie.gameObject));

        b.hitPosition = hitPoint.point;
        b.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        muzzleFlash.Play();
        AddRecoil();
        GetComponent<Recoil>().FireRecoilRotation((isAmining ? 0.5f : recoilRotationX),
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

        transform.parent.GetComponent<WeponsHolder>().StartReloadAnimation(reloadWeaponDir);
        anim.SetTrigger("Reload");
        isReloading = true;
    }

    public void Reloaded()
    {
        EndReload();
        int amount = (maxAmmo < mag ? maxAmmo : mag) - ammo;
        ammo += amount;
        mag -= amount;
    }

    public void EndReload()
    {
        isReloading = false;
        anim.Play("Idle");
    }

    IEnumerator DestroyAfterTime(GameObject gb)
    {
        yield return new WaitForSeconds(1f);
        Destroy(gb);
    }
}
