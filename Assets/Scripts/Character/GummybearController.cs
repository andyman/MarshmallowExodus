using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GummybearController : MonoBehaviour
{
	public LayerMask enemyLayerMask;
	public const int AI_SCANNING = 0;
	public const int AI_CHASE = 1;
	public const int AI_EATING = 2;

	public float speed = 5.0f;
	public int aiState = 0;

	private float nextScanTime;

	public float scanRadius = 30.0f;

	public Transform prey = null;
	private Collider[] results;
	private Rigidbody rb;

	public AudioClip[] growlSounds;
	private float nextGrowlTime;
	private AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
		results = new Collider[32];
		rb = GetComponent<Rigidbody>();
		myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextScanTime)
		{
			nextScanTime = Time.time + 1.0f;
			int resultCount = Physics.OverlapSphereNonAlloc( transform.position, scanRadius, results, enemyLayerMask);
			Transform nearestPrey = null;
			float nearestDistance = Mathf.Infinity;
			Vector3 myPos = transform.position;
			bool found = false;

			for(int i = 0; i < resultCount; i++)
			{
				Transform t = results[i].transform;
				float dist = Vector3.Distance(t.position, myPos);
				if (dist < nearestDistance || i == 0)
				{
					nearestPrey = t;
					nearestDistance = dist;
					found = true;
				}
			}
			if (!found)
			{
				prey = null;
			}
			else
			{
				prey = nearestPrey;
				if (nearestDistance < 4.0f)
				{
					Vector3 preyPos = prey.position;
					preyPos.y = transform.position.y;
					transform.LookAt(preyPos);
					DeathManager.Die(prey);
					nextScanTime = Time.time + 2.0f;
				}
			}
		}

		if (prey != null && Time.time > nextGrowlTime)
		{
			nextGrowlTime = Time.time + Random.Range(3.0f, 6.0f);
			myAudioSource.clip = growlSounds[Random.Range(0, growlSounds.Length)];
			myAudioSource.pitch = Random.Range(0.5f, 0.6f);
			myAudioSource.Play();
		}
	}

	void FixedUpdate()
	{
		Vector3 v = rb.velocity;

		if (prey == null)
		{
			v = Vector3.zero;
		}
		else
		{
			Vector3 targetPos = prey.position;
			Vector3 diff = targetPos - transform.position;
			float dist = diff.magnitude;
			diff.y = 0.0f;
			Vector3 dir = diff.normalized;

			v = dir * speed;

			Quaternion startRot = rb.rotation;
			Quaternion targetRot = Quaternion.LookRotation(dir);

			rb.rotation = Quaternion.Lerp(startRot, targetRot, 5.0f * Time.deltaTime);
		}

		v.y = rb.velocity.y;
		rb.velocity = v;
	}
}
