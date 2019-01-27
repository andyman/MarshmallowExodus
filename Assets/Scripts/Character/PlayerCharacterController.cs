using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{

		public float maxSpeed = 4.0f;
	public float acceleration = 8.0f;
	public float jumpSpeed = 10.0f;
		private Rigidbody rb;

	private Camera cam;

  		// Start is called before the first frame update
  		void Start()
    {
			rb = GetComponent<Rigidbody>();
			cam = Camera.main;
		myAudioSource = GetComponent<AudioSource>();
    }

	private float horizontalInput, verticalInput;
	private bool jumpPressed;
	private bool jumpHeld;
	public bool grounded;
	public float fallGravityMultiplier = 0.1f;
	public static PlayerCharacterController instance;
    public float ragdollUntilTime;

	private AudioSource myAudioSource;
	public AudioClip[] syllables;

	public void OnEnable()
	{
		instance = this;
	}
	public void OnDisable()
	{
		instance = null;
	}
	public Animator anim;
    public float armsUp = 0.0f;

    [Range(0.0f, 1.0f)]
    public float happiness = 0.5f;
    public SkinnedMeshRenderer skinMR;

	// Update is called once per frame
	private void Update()
	{
        int weightIndex = happiness >= 0.5f ? 0 : 1;
        float blendHappiness = Mathf.Abs(happiness - 0.5f) * 100.0f;

        skinMR.SetBlendShapeWeight(weightIndex, blendHappiness);
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");
		jumpPressed = Input.GetButtonDown("Jump");
		jumpHeld = Input.GetButton("Jump");

        if (Input.GetButton("Fire3"))
        {
            armsUp = Mathf.Lerp(armsUp, 1.0f, Time.deltaTime * 5.0f);

			if (!myAudioSource.isPlaying)
			{
				myAudioSource.clip = syllables[Random.Range(0, syllables.Length)];
				myAudioSource.pitch = Random.Range(0.9f, 1.1f);
				myAudioSource.volume = Random.Range(0.3f, 0.5f);
				myAudioSource.Play();
			}
		}
        else
        {
            armsUp = Mathf.Lerp(armsUp, 0.0f, Time.deltaTime * 5.0f);
        }

        anim.SetFloat("arms", armsUp);




		if (transform.position.y < -100.0f)
		{
			DeathManager.Die(transform);
		}

	}

	public LayerMask groundLayer;
	public Transform groundCheckCenter;

	void FixedUpdate()
	{
		grounded = Physics.CheckSphere(groundCheckCenter.position, 0.5f, groundLayer);
		if (Time.time < ragdollUntilTime)
        {
            return;
        }
			Vector3 direction = cam.transform.rotation * (new Vector3(horizontalInput, 0.0f, verticalInput));
			direction.y = 0.0f; // flatten
			direction.Normalize();

		Vector3 v = rb.velocity;

		bool isMovingPressed = Mathf.Abs(horizontalInput) > 0.01f || Mathf.Abs(verticalInput) > 0.01f;

		if (!isMovingPressed)
		{
			v = Vector3.zero;
		}
		else
		{
			Vector3 targetV = direction * maxSpeed;
			v = Vector3.Lerp(v, targetV, acceleration * Time.deltaTime);

			Quaternion startRot = rb.rotation;
			Quaternion targetRot = Quaternion.LookRotation(direction);

			rb.rotation = Quaternion.Lerp(startRot, targetRot, 5.0f * Time.deltaTime);
		}

		v.y = rb.velocity.y;

		if (jumpPressed && grounded)
		{ 
			jumpPressed = false;
			grounded = false;
			v.y += jumpSpeed;
		}

		if (!jumpHeld && !grounded)
		{
			v.y += Physics.gravity.y * fallGravityMultiplier;
		}

		rb.velocity = v;
    }
}
