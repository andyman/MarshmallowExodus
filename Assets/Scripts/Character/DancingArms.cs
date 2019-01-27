using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingArms : MonoBehaviour
{
	private Animator anim;

	private float targetArm;
	private float nextArmTime;
	private float arms;
	private SkinnedMeshRenderer sm;

    // Start is called before the first frame update
    void Start()
    {
        if (anim == null)
		{
			anim = GetComponentInChildren<Animator>();
		}
		arms = Random.value;

		sm = GetComponentInChildren<SkinnedMeshRenderer>();
		happiness = Random.value;
	}

	float happiness, targetHappiness;

    // Update is called once per frame
    void Update()
    {
    	if (Time.time > nextArmTime)
		{
			targetArm = Random.value;
			nextArmTime = Time.time + Random.Range(0.25f, 0.5f);
			targetHappiness = Random.Range(-1f, 1.0f);
		}

		arms = Mathf.Lerp(arms, targetArm, Time.deltaTime * 5.0f);

		anim.SetFloat("arms", arms);

		happiness = Mathf.Lerp(happiness, targetHappiness, Time.deltaTime * 1.0f);

		int weightIndex = happiness >= 0.5f ? 0 : 1;
		float blendHappiness = Mathf.Abs(happiness - 0.5f) * 200.0f;

		sm.SetBlendShapeWeight(weightIndex, blendHappiness);



	}
}
