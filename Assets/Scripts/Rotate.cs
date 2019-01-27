using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField]
    private Vector3 rotateVector = Vector3.up * 100;

    private void Update()
    {
        transform.Rotate(rotateVector * Time.deltaTime, Space.Self);
    }

}
