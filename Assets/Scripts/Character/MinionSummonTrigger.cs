using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSummonTrigger : MonoBehaviour
{
	public SphereCollider trigger;

    // Start is called before the first frame update
    void Start()
    {
		trigger.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
		trigger.enabled = Input.GetButton("Fire3");

	}

	void OnTriggerStay(Collider other)
	{
		MinionController minion = other.GetComponent<MinionController>();
		if (minion != null)
		{
			minion.followUntilTime = Time.time + 0.25f;
		}
	}
}
