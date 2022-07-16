using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private Vector3 currentPosition;
    private Vector3 targetposition;

    [SerializeField] Vector3 actualRotation = Vector3.zero;
    [SerializeField] Vector3 actualPositionn = Vector3.zero;
    [SerializeField] float snappiness;
    [SerializeField] float returnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, actualRotation, returnSpeed * Time.deltaTime);
        targetposition = Vector3.Lerp(targetposition, actualPositionn, returnSpeed * Time.deltaTime);

        currentRotation = Vector3.Slerp(currentRotation ,  targetRotation , snappiness * Time.fixedDeltaTime);
        currentPosition = Vector3.Lerp(currentPosition , targetposition, snappiness * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
        transform.localPosition = currentPosition;
    }

    public void FireRecoilRotation(float x , float y , float z)
    {
        targetRotation += new Vector3(-x, Random.Range(-y, y), Random.Range(-z, z));
    }

    public void FireRecoilPosition(float x, float y, float z)
    {
        targetposition += new Vector3(Random.Range(-x, x), Random.Range(0, y), -z);
    }
}
