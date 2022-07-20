using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWeaponsManager : MonoBehaviour
{
    public MapWeapon[] weapons;

    int weaponIndex;
    public bool canBuy = false;

    bool buyFlag = false; 

    public static MapWeaponsManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    { 
        for (int i = 0; i < weapons.Length; i ++)
        {
            if (weapons[i].canBuy)
            {
                buyFlag = true;
                weaponIndex = i;
                int price = weapons[i].isWithPlayer ? weapons[i].ammoPrice : weapons[i].price;
                UiGameManager.instance.ShowMapWeaponHintMessange("Buy " + weapons[i].name +
                    ( weapons[i].isWithPlayer ? " Ammo" : "") + " For " + price + " Point");
            }
        }
        canBuy = buyFlag;
        buyFlag = false;
    }

    public MapWeapon BuyWeapon()
    {
        return weapons[weaponIndex];
    }
}
