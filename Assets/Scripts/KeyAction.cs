using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyAction : MonoBehaviour
{
	public UnityEvent keyEvent;
	public KeyCode key;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	if (Input.GetKeyDown(key))
		{
			keyEvent.Invoke();
		}
	}
}
