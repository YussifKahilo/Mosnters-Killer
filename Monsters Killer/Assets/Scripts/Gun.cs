using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations.Rigging;

public class Gun : MonoBehaviour
{
    public GunData data;

    [SerializeField] TextMeshProUGUI weaponAmmoTextNormal , weaponAmmoTextAlert, weaponMagText;
    [SerializeField] Transform barel;
    [SerializeField] Transform cam;
    float cursorSize;
    public RectTransform cursor;
    [SerializeField] ParticleSystem muzzleFlash;

    [SerializeField] Transform handsConstraint;
    [HideInInspector] public TwoBoneIKConstraint rightHandConstraint, leftHandConstraint;


    Animator anim;
    public bool isReloading = false;
    public bool canShoot = true;

    float targetRecoilSize;
    float currentRecoilSize;

    float targetCursorSize;
    float currentCursorSize;

    float recoilAmount;

    private void OnEnable()
    {
        if (data.ammo == 0)
        {
            Reload();
        }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();

        rightHandConstraint = handsConstraint.GetChild(0).GetComponent<TwoBoneIKConstraint>();
        leftHandConstraint = handsConstraint.GetChild(1).GetComponent<TwoBoneIKConstraint>();

        SetCursor();
        RestWeapon();
    }

    void SetCursor()
    {
        cursorSize = 3600 * data.recoil;
        targetCursorSize = cursorSize;
        currentCursorSize = cursorSize;
        targetRecoilSize = data.recoil;
        currentRecoilSize = data.recoil;
        recoilAmount = data.recoil;
    }

    public void RestWeapon()
    {
        data.mag = data.maxMag;
        data.ammo = data.maxAmmo;
    }

    private void Update()
    {
        recoilAmount = data.recoil + (GameManager.instance.playerManager
            .GetComponent<PlayerMovement>().movement.magnitude > 0 ? 0.05f : 0);
        

        canShoot = !isReloading;
        cursorSize = recoilAmount * 3600;

        weaponAmmoTextNormal.gameObject.SetActive(!(data.ammo <= data.maxAmmo / 4));
        weaponAmmoTextAlert.gameObject.SetActive(data.ammo <= data.maxAmmo / 4);

        weaponAmmoTextNormal.text = data.ammo + "";
        weaponAmmoTextAlert.text = data.ammo + "";

        weaponMagText.text = data.mag + "";
        
        if (enabled)
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
        if (data.ammo == 0)
            Reload();

        data.ammo--;
        Bullet b = Instantiate(bullet, barel.position, Quaternion.identity).GetComponent<Bullet>();


        Vector3 recoilAddVector = transform.parent.GetComponent<WeponsHolder>().isAiming ?
            new Vector3(Random.Range(-data.aimRecoil, data.aimRecoil), Random.Range(-data.aimRecoil, data.aimRecoil),
            Random.Range(-data.aimRecoil, data.aimRecoil)) :
            new Vector3(Random.Range(-currentRecoilSize, currentRecoilSize),
            Random.Range(-currentRecoilSize, currentRecoilSize), Random.Range(-currentRecoilSize, currentRecoilSize));

        RaycastHit hitPoint;
        Ray ray = new Ray(cam.position, cam.forward + recoilAddVector);
        Physics.Raycast(ray, out hitPoint, 100f, data.layers);

        ParticleSystem ie = Instantiate(data.impactEffect, hitPoint.point,
            Quaternion.LookRotation(hitPoint.normal)).GetComponent<ParticleSystem>();
        ie.Play();
        StartCoroutine(DestroyAfterTime(ie.gameObject , 4));

        b.hitPosition = hitPoint.point;
        b.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        muzzleFlash.Play();
        AddRecoil();
        GetComponent<Recoil>().FireRecoilRotation((isAmining ? 0.5f : data.recoilRotationX),
            (isAmining ? 0.5f : data.recoilRotationY), (isAmining ? 0.5f : data.recoilRotationZ));
        GetComponent<Recoil>().FireRecoilPosition((isAmining ? 0.001f : data.recoilPositionX),
            (isAmining ? 0.001f : data.recoilPositionY), (isAmining ? 0.001f : data.recoilPositionZ));

        if (data.ammo == 0)
        {
            Reload();
        }
    }

    public void Reload()
    {
        if (data.mag == 0 || data.ammo == data.maxAmmo || isReloading)
            return;

        transform.parent.GetComponent<WeponsHolder>().StartReloadAnimation(data.reloadWeaponDir);
        anim.SetTrigger("Reload");
        isReloading = true;
    }

    public void Reloaded()
    {
        EndReload();
        int amount = (data.maxAmmo < data.mag ? data.maxAmmo : data.mag) - data.ammo;
        data.ammo += amount;
        data.mag -= amount;
    }

    public void EndReload()
    {
        isReloading = false;
        anim.Play("Idle");
    }

    public bool IsFullAmmo()
    {
        return data.mag == data.maxMag && data.ammo == data.maxAmmo;
    }

    IEnumerator DestroyAfterTime(GameObject gb , float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gb);
    }
}
