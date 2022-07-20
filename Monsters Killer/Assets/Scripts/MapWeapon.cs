using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWeapon : MonoBehaviour
{
    public int weaponId;

    [SerializeField] Transform weaponBody;
    [SerializeField] float distanceFromPlayer = 1.1f;

    public int price;
    public int ammoPrice;

    PlayerManager playerManager;

    public bool isWithPlayer = false;
    public bool canBuy = false;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameManager.instance.playerManager;
    }

    // Update is called once per frame
    void Update()
    {
        weaponBody.Rotate(0, 20 * Time.deltaTime ,0);

        float distance = Vector3.Distance(transform.position, playerManager.transform.position);

        canBuy = distance <= distanceFromPlayer;
    }
}
