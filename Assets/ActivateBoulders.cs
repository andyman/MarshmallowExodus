using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBoulders : MonoBehaviour
{
    SpawnRocksAbove spawnRocksAbove;

    // Start is called before the first frame update
    void Start()
    {
        spawnRocksAbove = gameObject.GetComponent<SpawnRocksAbove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            spawnRocksAbove.enabled = true;//Enables falling bombs
        }
    }

}
