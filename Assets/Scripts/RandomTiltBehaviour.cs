using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTiltBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    private float endTime;
    private float startTime;
    private Vector2 startTilt;
    private Vector2 endTilt;
    public float minDuration;
    public float maxDuration;
    public string tiltX;
    public string tiltY;

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time > endTime)
        {
            startTime = Time.time;
            endTime = Time.time + Random.Range(minDuration, maxDuration);
            startTilt = new Vector2(animator.GetFloat(tiltX), animator.GetFloat(tiltY));
            endTilt = Random.insideUnitCircle;
        }

        float timeLerp = Mathf.InverseLerp(startTime, endTime, Time.time);
        Vector2 tilt = Vector2.Lerp(startTilt, endTilt, timeLerp);

        animator.SetFloat(tiltX, tilt.x);
        animator.SetFloat(tiltY, tilt.y);

    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
