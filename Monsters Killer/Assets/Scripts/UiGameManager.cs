using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiGameManager : MonoBehaviour
{
    [SerializeField] GameObject androidPanel;

    public void OnPause(InputAction.CallbackContext value)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        androidPanel.SetActive(Application.platform == RuntimePlatform.Android);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
