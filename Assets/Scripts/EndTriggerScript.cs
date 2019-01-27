using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTriggerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerEnter(Collider other)
	{
		int layer = other.gameObject.layer;

		if (layer == 10) // player
		{
			LevelManager.instance.endReached = true;
		}
		else if (layer == 14) // bear
		{
			// TODO kill bear - effect & sound

			Destroy(other.gameObject);
		}
	}
}
