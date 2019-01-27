using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoulders : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroyAfterLifeTime();
    }

    private void DestroyAfterLifeTime()
    {
        Destroy(gameObject, 10f);
    }
}
