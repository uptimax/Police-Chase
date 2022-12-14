using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum AIAction{
    Jumping,
    Climbing,
    Sliding,
    Dropping,
    HardDropping
}

public class AiActionTrigger : MonoBehaviour
{
    public GameObject ObstacleAvoider;
    [SerializeField] private AIAction actionType;
    private bool triggered;
   
    private void OnTriggerEnter(Collider collider){
        print("super mario");
        if(collider.CompareTag("Police") && !triggered){
            PoliceAI _policeAi = collider.GetComponent<PoliceAI>();

            if(!(actionType == AIAction.Sliding))
            _policeAi.ApplyRootMotion();

            _policeAi.ObstacleAvoider = ObstacleAvoider;
            triggered = true;
            print(collider.gameObject);

            switch(actionType){
                case AIAction.Climbing:
                _policeAi.Climb();
                break;
                case AIAction.Jumping:
                _policeAi.Jump();
                break;
                case AIAction.Sliding:
                _policeAi.Slide();
                break;
                case AIAction.Dropping:
                _policeAi.Drop();
                break;
                case AIAction.HardDropping:
                _policeAi.HardDrop();
                break;
            }
        }
    }
}
