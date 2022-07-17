using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeponsHolder : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Gun[] guns;
    [SerializeField] Camera cam;
    [SerializeField] TextMeshProUGUI screenAmmpText;
    [SerializeField] Recoil headRecoil;

    Gun firstGun;
    Gun secondGun;

    int changeIndex = -1;
    float changeTimeElapsed = 0;
    bool loweringWeight = false, higheringWeight = false;

    public PlayerMovement playerMovement;
    [SerializeField] Vector3 normalHandPosition;
    [SerializeField] Vector3 aimHandPosition;

    [SerializeField]float recoilX = 2 , recoilY = 2 , recoilZ = 2;

    Animator anim;

    bool isFireing = false;
    bool isChangingWeapon = false;
    public bool isAiming = false;

    public bool aimGun = false;

    public bool changeAimView = false;
    float aimLerpDuration = 0.1f;
    float aimTimeElapsed;

    float aimFov = 40;
    float noramlFov = 60;


    float nextTimeToFire ;

    public void OnFire(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            isFireing = true;
            playerMovement.senstivity /= 2;
        }
        else if (value.canceled)
        {
            isFireing = false;
            playerMovement.senstivity *= 2;
        }
    }

    public void OnReload(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            CurrentGun().Reload();
        }
    }

    public void OnWeaponOneChange(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            ChangeWeapon(-1);
        }
    }

    public void OnWeaponTwoChange(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            ChangeWeapon(1);
        }
    }

    public void OnJoystickWeaponChange(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            ChangeWeapon(changeIndex == -1 ? 1 : -1);
        }
    }

    public void OnScrollWeaponChange(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            ChangeWeapon((int)value.ReadValue<Vector2>().normalized.y);
        }
    }

    public void ChangeWeapon(int num)
    {
        if (changeIndex == num || isChangingWeapon)
            return;

        if(CurrentGun().isReloading)
            CurrentGun().EndReload();

        isChangingWeapon = true;
        changeIndex = num;
        anim.SetTrigger("Change Weapon");
        loweringWeight = true;
    }

    public void OnAim(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            StartAim();

        }else if (value.canceled && Gamepad.current != null)
        {
            StopAim();
        }
    }

    void StartAim()
    {
        if (!playerMovement.isGrounded || playerMovement.speed > 3)
            return;

        isAiming = !isAiming;
        aimGun = isAiming;
        if (isAiming)
        {
            playerMovement.senstivity /= 3;
            recoilX /= 2;
            recoilY /= 2;
            recoilZ /= 2;
        }
        else
        {
            playerMovement.senstivity *= 3;
            recoilX *= 2;
            recoilY *= 2;
            recoilZ *= 2;
        }

        if (!CurrentGun().isReloading)
            changeAimView = true;
    }

    public void StopAim()
    {
        isAiming = false;
        aimGun = isAiming;
        playerMovement.senstivity *= 3;
        recoilX *= 2;
        recoilY *= 2;
        recoilZ *= 2;

        if (!CurrentGun().isReloading)
            changeAimView = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
        firstGun = guns[0];
        secondGun = guns[1];
    }

    // Update is called once per frame
    void Update()
    {
        Aiming();
        if (isChangingWeapon)
        {
            ChangeWeights();
            return;
        }

        if (isFireing && CurrentGun().canShoot)
        {
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / CurrentGun().fireRate;

                headRecoil.FireRecoilRotation(isAiming? 0.5f : recoilX, isAiming ? 0.5f :
                    recoilY, isAiming ? 0.5f : recoilZ);
                CurrentGun().Shot(bullet , isAiming);
            }
        }
    }

    void ChangeWeights()
    {
        if (loweringWeight)
        {
            if (changeTimeElapsed < 0.4f)
            {
                PrevGun().leftHandConstraint.weight = Mathf.Lerp(1 , 0 , changeTimeElapsed / 0.4f);
                //PrevGun().rightHandConstraint.weight = Mathf.Lerp(1, 0.5f, changeTimeElapsed / 0.4f);

                //CurrentGun().rightHandConstraint.weight = Mathf.Lerp(0, 0.5f, changeTimeElapsed / 0.4f);
                changeTimeElapsed += Time.deltaTime;
            }
            else
            {
                PrevGun().leftHandConstraint.weight = 0;

                PrevGun().rightHandConstraint.weight = 0f;
                CurrentGun().rightHandConstraint.weight = 1f;

                changeTimeElapsed = 0;
                loweringWeight = false;
            }
        }

        if (higheringWeight)
        {
            if (changeTimeElapsed < 0.4f)
            {
                CurrentGun().leftHandConstraint.weight = Mathf.Lerp(0, 1, changeTimeElapsed / 0.4f);

                changeTimeElapsed += Time.deltaTime;
            }
            else
            {
                CurrentGun().leftHandConstraint.weight = 1;
                changeTimeElapsed = 0;
                higheringWeight = false;
                isChangingWeapon = false;
            }
        }
    }

    void HighLeftWeight()
    {
        higheringWeight = true;
    }

    void Aiming()
    {

        if ((CurrentGun().isReloading || isChangingWeapon )&& aimGun && isAiming)
        {
            isAiming = !isChangingWeapon;
            aimGun = false;
            changeAimView = true;
        }
        else if (!CurrentGun().isReloading && !aimGun && isAiming)
        {
            aimGun = true;
            changeAimView = true;
        }

        if (changeAimView)
        {
            if (aimTimeElapsed < aimLerpDuration)
            {
                transform.localPosition = Vector3.Lerp(aimGun ? normalHandPosition : aimHandPosition,
                    aimGun ? aimHandPosition : normalHandPosition, aimTimeElapsed / aimLerpDuration);
                cam.fieldOfView = Mathf.Lerp(aimGun ? noramlFov : aimFov, aimGun ? aimFov : noramlFov, aimTimeElapsed / aimLerpDuration);

                aimTimeElapsed += Time.deltaTime;

            }
            else
            {
                cam.fieldOfView = aimGun ? aimFov : noramlFov;
                transform.localPosition = aimGun ? aimHandPosition : normalHandPosition;
                aimTimeElapsed = 0;
                changeAimView = false;
            }
        }
    }

    public void SwitchWeapon()
    {
        CurrentGun().gameObject.SetActive(true);
        PrevGun().gameObject.SetActive(false);
        higheringWeight = true;
    }

    public void ChangeAnimationEnded()
    {
        //anim.SetTrigger("Change Weapon");
    }

    public void StartReloadAnimation(string animName)
    {
        anim.SetTrigger("Reload " + animName);
    }

    Gun PrevGun()
    {
        return changeIndex == 1 ? firstGun : secondGun;
    }

    Gun CurrentGun()
    {
        return changeIndex == 1 ? secondGun : firstGun;
    }
}
