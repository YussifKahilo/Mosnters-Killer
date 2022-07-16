using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderAnimationEvents : MonoBehaviour
{
    public void SwitchWeapon()
    {
        transform.GetChild(0).GetComponent<WeponsHolder>().SwitchWeapon();
    }

    public void ChangeAnimationEnded()
    {
        transform.GetChild(0).GetComponent<WeponsHolder>().ChangeAnimationEnded();
    }
}
