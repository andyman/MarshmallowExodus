using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTrapTrigger : MonoBehaviour
{
    [SerializeField] GameObject rockGate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            rockGate.SetActive(false);
        }
    }
}
