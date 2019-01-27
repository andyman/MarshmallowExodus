using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
	public ParticleSystem deathParticleSystem;
	public static DeathManager instance;

	public AudioClip[] minionDeathSounds;
	public AudioClip[] leaderDeathSounds;

	public AudioSource audioSourcePrefab;


	public int deathCount = 0;
	private void OnEnable()
	{
		instance = this;
	}
	private void OnDisable()
	{
		if (instance == this)
			instance = null;
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public static void Die(Transform victim)
	{
		if (instance == null)
		{
			Debug.LogWarning("Add the DeathManager prefab to the scene!");
			return;
		}
		int vicLayer = victim.gameObject.layer;
		Vector3 vicLocation = victim.position;

		if (vicLayer == 10) // player
		{
			instance.deathParticleSystem.transform.position = victim.position;
			instance.deathParticleSystem.Emit(100);

			instance.deathCount++;

			// find nearest minion
			MinionController nearestMinion = MinionController.FindNearestMinion(victim.position);

			// kill minion
			if (nearestMinion != null)
			{
				Vector3 spawnPos = nearestMinion.transform.position;
				nearestMinion.gameObject.SetActive(false);
				Destroy(nearestMinion.gameObject);

				// swap player with minion
				victim.position = spawnPos;
			}
			else
			{
				victim.gameObject.SetActive(false);
				if (LevelManager.instance == null)
				{
					Debug.LogWarning("Add the LevelManager prefab to the scene!");
				}
				else
				{
					LevelManager.instance.playerDead = true;
				}
			}

			AudioSource deathcry = Instantiate(instance.audioSourcePrefab, vicLocation, Quaternion.identity);
			deathcry.clip = instance.minionDeathSounds[Random.Range(0, instance.minionDeathSounds.Length)];
			deathcry.pitch = Random.Range(0.9f, 1.1f);
			deathcry.Play();
			Destroy(deathcry, deathcry.clip.length + 5.0f);


		}
		else if (vicLayer == 11) // minion
		{
			AudioSource deathcry = Instantiate(instance.audioSourcePrefab, victim.position, Quaternion.identity);
			deathcry.clip = instance.minionDeathSounds[Random.Range(0, instance.minionDeathSounds.Length)];
			deathcry.pitch = Random.Range(0.9f, 1.1f);
			deathcry.Play();
			Destroy(deathcry, deathcry.clip.length + 5.0f);

			instance.deathParticleSystem.transform.position = victim.position;
			instance.deathParticleSystem.Emit(100);
			instance.deathCount++;
			victim.gameObject.SetActive(false);
			Destroy(victim.gameObject);
		}


	}
}
