using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiGameManager : MonoBehaviour
{

    public static UiGameManager instance;

    public void OnPause(InputAction.CallbackContext value)
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
        Debug.Log(msg);
    }

}
