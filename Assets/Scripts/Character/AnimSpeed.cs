using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSpeed : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    private float lastSpeed;
    private Vector3 lastPos;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 diff = pos - lastPos;
        float speed = diff.magnitude / Time.deltaTime;
        lastSpeed = Mathf.Lerp(lastSpeed, speed, Time.deltaTime * 10.0f);
        anim.SetFloat("speed", lastSpeed);
        lastPos = pos;
   
    }

}
