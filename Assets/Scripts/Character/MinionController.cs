using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
	public Transform groundCheckCenter;
	public Transform obstructionCheckCenter;
	public Transform forwardGroundCheckCenter;

	public bool grounded;
	public bool forwardGroundExists;
	public bool forwardObstructionExists;

	public Transform target; // walk towards target if there is one
	public float followUntilTime;
	public float speed;
    public float ragdollUntilTime;
	public float jumpHeight;
	public Rigidbody rb;
	private float noJumpUntilTime;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
    }
	public static MinionController FindNearestMinion(Vector3 pos)
	{
		MinionController result = null;
		Vector3 myFlatPosition = pos;
		myFlatPosition.y = 0.0f;

		float nearestDistance = Mathf.Infinity;
		for(int i = 0; i < minionsList.Count; i++)
		{
			Vector3 minionFlatPos = minionsList[i].transform.position;
			minionFlatPos.y = 0.0f;

			float dist = Vector3.Distance(minionFlatPos, myFlatPosition);

			if (dist > 40.0f)
				continue;

			if (dist < nearestDistance || i == 0)
			{
				result = minionsList[i];
			}
		}

		return result;
	}

	// Update is called once per frame
	void Update()
    {
		if (target == null && PlayerCharacterController.instance != null)
		{
			target = PlayerCharacterController.instance.transform;
		}

		if (transform.position.y < -100.0f)
		{
			DeathManager.Die(transform);
		}
	}

	public static List<MinionController> minionsList = new List<MinionController>();

	private void OnEnable()
	{
		minionsList.Add(this);
	}
	private void OnDisable()
	{
		minionsList.Remove(this);
	}

	public LayerMask groundCheckLayer;

	void FixedUpdate()
	{
		grounded = Physics.CheckSphere(groundCheckCenter.position, 0.1f, groundCheckLayer);
		forwardGroundExists = Physics.CheckSphere(forwardGroundCheckCenter.position, 0.25f, groundCheckLayer);
		forwardObstructionExists = Physics.CheckSphere(obstructionCheckCenter.position, 0.25f, groundCheckLayer);

        if (Time.time < ragdollUntilTime)
            return;

		Vector3 v = rb.velocity;
		bool jump = false;

		if (target != null && Time.time < followUntilTime)
		{
			// walk towards target

			Vector3 targetPos = target.position;
			Vector3 diff = targetPos - transform.position;
			float dist = diff.magnitude;
			Vector3 dir = rb.rotation * Vector3.forward;
			if (dist < 3.0f) // back up if too close
			{


				diff.y = 0.0f; // flatten the direction
				dir = -diff.normalized;

				v = dir * speed;
			}
			else if (dist > 5.0f)
			{

				diff.y = 0.0f; // flatten the direction

				dir = diff.normalized;

				v = dir * speed;
			}
			else
			{
				// stop
				v = Vector3.zero;
			}

			if (v != Vector3.zero)
			{
				// turning

				Vector3 targetV = dir * speed;
				v = Vector3.Lerp(v, targetV, 8.0f * Time.deltaTime);

				Quaternion startRot = rb.rotation;
				Quaternion targetRot = Quaternion.LookRotation(dir);

				rb.rotation = Quaternion.Lerp(startRot, targetRot, 5.0f * Time.deltaTime);

				// jumping

				jump = (grounded && (!forwardGroundExists || forwardObstructionExists));
			}
		}
		else
		{
			// stop
			v = Vector3.zero;
		}

		v.y = rb.velocity.y; // maintain up/down velocity
		if (jump && Time.time > noJumpUntilTime)
		{
			grounded = false;
			v.y += jumpHeight;
			noJumpUntilTime = Time.time + 1f;
			lastJumpTime = Time.time;
		}

		// slam down after some time
		if (!grounded && Time.time > (lastJumpTime+0.75f))
		{
			v.y += Physics.gravity.y * 0.1f;
		}

		rb.velocity = v;
	}
	float lastJumpTime;
}
