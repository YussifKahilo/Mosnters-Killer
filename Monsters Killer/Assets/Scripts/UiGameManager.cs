using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UiGameManager : MonoBehaviour
{
    [SerializeField] Text weaponHintText;


    public static UiGameManager instance;

    public void PauseGame()
    {

    }

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        
    }

    public void ShowMapWeaponHintMessange(string msg)
    {
        weaponHintText.text = msg;
    }

}
