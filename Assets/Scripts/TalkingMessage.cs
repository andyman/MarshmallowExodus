using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingMessage : MonoBehaviour
{
	private AudioSource myAudioSource;
	public AudioClip[] syllables;

	// Start is called before the first frame update
	void Start()
    {
		myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		if (!myAudioSource.isPlaying)
		{
			myAudioSource.clip = syllables[Random.Range(0, syllables.Length)];
			myAudioSource.pitch = Random.Range(0.9f, 1.1f);
			myAudioSource.volume = Random.Range(0.3f, 0.5f);
			myAudioSource.Play();
		}
	}
}
