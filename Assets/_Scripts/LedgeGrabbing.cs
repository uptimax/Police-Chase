using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    
    [Header("References")]
    public PlayerController playerController;
    public CharacterController controller;
    public Transform cameraPosition;
    public Transform orientation;

    [Header("Ledge Grabbing")]
    public float moveToLedgeSpeed;
    public float maxLedgeGrabDistance;
    public float  minTimeOnLedge;
    public bool holding;

    [Header("Ledge Detection")]
    public float ledgeDetectionLength;
    public float ledgeSphereCastRadius;
    public LayerMask whatIsLedge;

    private float timeOnLedge;
    private float coolDownTime = 0f;


    private Transform lastLedge;
    private Transform currLedge;

    private RaycastHit ledgeHit;


    private void LedgeDetection(){
        bool ledgeDetection = Physics.SphereCast(transform.position, ledgeSphereCastRadius, cameraPosition.right, out ledgeHit, ledgeDetectionLength, whatIsLedge);
        if(!ledgeDetection) return;

        float distanceToLedge = Vector3.Distance(transform.position, ledgeHit.transform.position);

        if(ledgeHit.transform == lastLedge) return;
        if(distanceToLedge < maxLedgeGrabDistance && !holding) EnterLedgeHold();
    }

    private void EnterLedgeHold(){
        holding = true;

        // pm.unlimited = true;
        // pm.restricted = true;

        playerController.RestrictSwiping();

        currLedge = ledgeHit.transform;
        lastLedge = ledgeHit.transform;

        //disable player gravity
        playerController.DisableGravity();
        playerController.EnableFreeze();

        //disable player velocity

    }

    private void subStateMachine(){
        bool playerSwipeScreen = MobileInput.Instance.SwipeUp || MobileInput.Instance.SwipeDown;


        //SubState 1 - Holding onto ledge
        if(holding){
            FreezeRigidbodyOnLedge();

            timeOnLedge += Time.deltaTime;
            if(timeOnLedge > minTimeOnLedge && playerSwipeScreen) ExitLedgeHold();
        }
    }
    private void FreezeRigidbodyOnLedge(){
        // freeze user gravity

        Vector3 directionToLedge = currLedge.position - transform.position;
        float distanceToLedge = Vector3.Distance(transform.position, currLedge.position);

        if(distanceToLedge > 1f){
            if(controller.velocity.magnitude < moveToLedgeSpeed)
            controller.Move(directionToLedge);
        }
        // Hold onto ledge
        else{
            // if(!pm.freeze) pm.freeze = true;
            // if(!pm.unlimited) player_movement.unlimited = false;

            if(!playerController.GetFreeze()) playerController.EnableFreeze();
            // if(!playerController.unlimited) playerController.g;
        }

        // Exiting if something goes wrong
        if(distanceToLedge > maxLedgeGrabDistance) ExitLedgeHold();

    }
    private void ExitLedgeHold(){
        print("super");
        holding = false;
        timeOnLedge = 0f;

        // pm.restricted = false;
        // pm.freeze = false;

        playerController.UnrestrictSwiping();
        playerController.DisableFreeze();

        playerController.EnableGravity();
        // playerController.Jump(15.0f);

        StopAllCoroutines();
        Invoke(nameof(ResetLastLedge), 1f);
    }

    private void ResetLastLedge(){
        lastLedge = null;
    }

    private void FixedUpdate() {
        LedgeDetection();
        subStateMachine();
    }


}
