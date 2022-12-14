using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ClimbingType{
    FREE
}
public enum TransitionState{
    NONE
}
public class IKEffector : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] Dictionary<string, float> ikEventTimes;
    [SerializeField] Dictionary<string, float> ikEffectorDelayTime;
    [SerializeField] Dictionary<string, float> ikEffectorLagTime;
    [SerializeField] Dictionary<string, Vector3> ikEffectorOffset;


    private float time;
    private String effector;

     private Dictionary<string, Dictionary<string ,GameObject>> ikTargets;

     private ClimbingType currentClimbType;
     private TransitionState currentTransitionState;
    //Effector States
    private string CURRENT;
    private string LAST;
    private string NEXT;
    private Dictionary<string, Dictionary<string, float>> ikRotationWeights;
    private bool jumpingDuringClimb;
    private float rightLegMaxReach;
    private float currentRightFootReach;
    private float currentLeftFootReach;

    private void Update(){
        bool reachedDestination = false;
        float afterTime = ikEventTimes [effector];
        afterTime += Time.deltaTime;
                    
        Vector3 newPosition;
        // delay and lag are used somewhat similarly, the important idea is sometimes we want to speed up or slow down an effector.
        // We sometimes also want to delay the start of interpolation but still reach the goal in time
        float delay = ikEffectorDelayTime [effector];
        time = time - delay + ikEffectorLagTime[effector];
        float adjustedAfterTime = Mathf.Clamp(afterTime - delay, 0f, time);
        float adjustedAfterTimeNoDelay = Mathf.Clamp(afterTime, 0f, time + delay);
        // this effector has reached the target
        if (adjustedAfterTime >= time) {
            newPosition = ikTargets[effector][NEXT].transform.position;
            reachedDestination = true;
            // character.SetParentTransform(ikTargets[effector][NEXT].transform.parent);
            ikEventTimes[effector] = 0f;
            ikTargets[effector][LAST].transform.position = newPosition;
            ikTargets[effector][LAST].transform.rotation = ikTargets[effector][NEXT].transform.rotation;
            ikTargets[effector][LAST].transform.parent = ikTargets[effector][NEXT].transform.parent;
        } else {
        // EaseLerp is a custom linear interpolation function with smoothing applied at the beginning and end of the motion.
            newPosition = EaseLerp(ikTargets[effector][LAST].transform.position,
                                            ikTargets[effector][NEXT].transform.position,
                                            adjustedAfterTime / time); 

            ikEventTimes[effector] = afterTime;
        }

        // All effectors (HAND, FOOT, ROOT) have a custom offset in the interpolation that is determined by the effector type and the current motion.
        newPosition += GetOffsetVectorForEffector(effector, Mathf.Clamp ((adjustedAfterTime / time) - 0.5f, -0.5f, 0.5f));

        Quaternion newRotation = Quaternion.Slerp (ikTargets [effector] [LAST].transform.rotation, ikTargets [effector] [NEXT].transform.rotation, (adjustedAfterTime / time));
                    ikRotationWeights [effector] [CURRENT] = (1f - Mathf.Sin ((adjustedAfterTime / time) * Mathf.PI)) * ((currentTransitionState == TransitionState.NONE) ? (currentClimbType == ClimbingType.FREE ? 0.75f : 0.1f) : 1f);

        // This code is just modifying the "stretch" of the legs during a jump
        if (effector.Contains ("FOOT") && this.jumpingDuringClimb) {
            float reachWeight = (1f - Mathf.Sin ((adjustedAfterTimeNoDelay / (time + delay)) * Mathf.PI));
            if (effector.Contains ("RIGHT")) {
                reachWeight *= rightLegMaxReach;
                currentRightFootReach = reachWeight;
            } else {
                reachWeight *= rightLegMaxReach;
                currentLeftFootReach = reachWeight;
            }
        }
        // This actually tells the IK system to set the effector target to the CURRENT transform position.
        SetIKTarget (effector, CURRENT, newPosition, newRotation, ikTargets [effector] [NEXT].transform.parent);
    }

    private void SetIKTarget(string effector, string cURRENT, Vector3 newPosition, Quaternion newRotation, Transform parent)
    {
        AvatarIKGoal IkGoal = AvatarIKGoal.LeftHand;

        switch(effector){
            case "LEFT_HAND":
            IkGoal = AvatarIKGoal.RightHand;
            break;
            case "RIGHT_HAND":
            IkGoal = AvatarIKGoal.LeftHand;
            break;
            case "RIGHT_FOOT":
            IkGoal = AvatarIKGoal.RightFoot;
            break;
            case "LEFT_FOOT":
            IkGoal = AvatarIKGoal.LeftFoot;
            break;
        }

        _animator.SetIKPosition(IkGoal, newPosition);
        _animator.SetIKRotation(IkGoal, newRotation);

         ikTargets[effector][cURRENT].transform.parent = parent;
    }

    private Vector3 EaseLerp(Vector3 position, Vector3 targetPosition, float duration){
        return Vector3.Lerp(position, targetPosition, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, duration)));
    }

    private Vector3 GetOffsetVectorForEffector(string effector, float duration){
        Vector3 currentPosition = ikTargets[effector][CURRENT].transform.position;
        Vector3 currentPositionWithOffset = currentPosition + (ikEffectorOffset[effector]);
        return Vector3.Lerp(-currentPosition, currentPositionWithOffset, duration);
    }
    

    
}
