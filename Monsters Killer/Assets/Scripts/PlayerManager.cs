using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public WeponsHolder weponsHolder;

    public int totalScore;
    public int score;
    public int kills;
    public int headShootKills;

    public void OnAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (MapWeaponsManager.instance.canBuy)
            {
                MapWeapon weapon = MapWeaponsManager.instance.BuyWeapon();

                if (weapon.isWithPlayer && weapon.ammoPrice <= score)
                {
                    if (weponsHolder.GetWeaponById(weapon.weaponId).IsFullAmmo())
                        return;

                    score -= weapon.ammoPrice;
                }
                else if (!weapon.isWithPlayer && weapon.price <= score)
                {
                    score -= weapon.price;
                }
                else
                {
                    return;
                }
                weponsHolder.ChangeWeapon(weapon);
            }
        }
    }

    public void OnPause(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            UiGameManager.instance.PauseGame();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
