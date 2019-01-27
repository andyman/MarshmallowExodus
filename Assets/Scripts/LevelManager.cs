using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public UnityEvent winEvent;
	public UnityEvent allDeadEvent;

	public bool playerDead = false;

	public bool gameOver = false;
	public bool endReached = false;
	public static LevelManager instance;
	
	public void OnEnable()
	{
		instance = this;
	}

	public void OnDisable()
	{
		if (instance == this)
			instance = null;
	}

	public float startTime;

	// Start is called before the first frame update
	void Start()
    {
		startTime = Time.time;
		    
    }

	public Text gameOverTitle;
	public Text gameOverSubtitle;
	public Text endValuesText;

	public int marshmallowsSaved;
	public int marshmallowsLost;
	public int journeyDuration;
	public int stepsWalked;

	public Text timeCounterLabel = null;

	// Update is called once per frame
	void Update()
	{
		float elapsedTime = Time.time - startTime;

		if (timeCounterLabel != null)
		{
			timeCounterLabel.text = Mathf.CeilToInt(elapsedTime).ToString();
		}

		if (!gameOver && playerDead)

		{
			gameOver = true;
			gameOverTitle.text = "Game Over";
			gameOverSubtitle.text = "You have found your final resting place.";
			FillStats();
			allDeadEvent.Invoke();

		}
		else if (!gameOver && endReached)
		{
			gameOver = true;
			gameOverTitle.text = "Victory!";
			gameOverSubtitle.text = "Home sweet home.";
			CountSaved();
			FillStats();
			allDeadEvent.Invoke();
		}

		if (gameOver)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene(0);
			}

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}
	}
	public PlayerCharacterController player;
	public LayerMask minionLayerMask;

	public void CountSaved()
	{
		Collider[] saved = Physics.OverlapSphere(player.transform.position, 40.0f, minionLayerMask);
		marshmallowsSaved = saved.Length + 1;

	}
	public void FillStats()
	{
		marshmallowsLost = DeathManager.instance.deathCount;

		// TODO - find how many marshmallows are in the safe space after x seconds

		journeyDuration = Mathf.CeilToInt(Time.time - startTime);
		//stepsWalked = Random.Range(500, 2000);

		endValuesText.text = marshmallowsSaved.ToString() + "\n" + marshmallowsLost + "\n "
			+ journeyDuration.ToString() ;
	}
}
