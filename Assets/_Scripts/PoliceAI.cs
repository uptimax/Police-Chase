using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using custEvents;

 public class AIEvent{
    public bool fired;
    public string eventName;
    public float onNormalizedTime = 1;
  } 

[RequireComponent(typeof(CharacterController))]
public class PoliceAI : MonoBehaviour
{
    [SerializeField] private GameObject PlayerTransform;
    public GameObject ObstacleAvoider{set; get;}

    private CharacterController _controller;
    
    private NavMeshAgent agent;
    private AgentLinkMover LinkMover;
    [SerializeField]private Animator _animator;

    [Header("Animation Settings")]
    public CrossFadeSettings _JumpSettings, _ClimbSettings, _StandToHangeSettings, _SlideSettings;
    public CrossFadeSettings _RunSettings, _JumpToHangSettings, _HangToClimbSettings;
    public CrossFadeSettings _DropSettings, _HardDropSettings;

    private FloatInterpolator speedInterpolator;
    private float speed = 2.8f, controllerSlidingHeight = 3.0f;
    private AnimationBlend AIAnimationBlender;

    //AI States
    private bool jumping, running, climbing, climbed, restrictMovement, sliding, chasing;
    public bool startChasing;

    public List<AIEvent>Events = new List<AIEvent>();
    public SMB_EventCurrator _eventCurrator;

    private void Start(){
        _animator = GetComponent<Animator>();
        _eventCurrator = GetComponent<SMB_EventCurrator>();
        _controller = GetComponent<CharacterController>();


        _eventCurrator.Event.AddListener(OnSMBEvent);

        speedInterpolator = new FloatInterpolator(min: 0, max: speed, smoothSpeed: 0.005f);

        AIAnimationBlender = new AnimationBlend(min: 0, max: 1, smoothSpeed: 0.005f, blendName: "Blend", blend: 0);

        _animator.applyRootMotion = false;
    }

    private void HandleLinkStart(OffMeshLinkMoveMethod moveMethod){
         if(moveMethod == OffMeshLinkMoveMethod.Parabola){
            jumping = true;
        }
    }

    private void HandleLinkEnd(OffMeshLinkMoveMethod moveMethod){
        print("handling end");
    }

    public void Slide(){ 
        _animator.CrossFadeInFixedTime(_SlideSettings);
    }
    public void Jump(){
        _animator.CrossFadeInFixedTime(_JumpSettings);
    }

    public void Climb(){
        _animator.CrossFadeInFixedTime(_JumpToHangSettings);
    }

    public void Drop(){
        _animator.CrossFadeInFixedTime(_DropSettings);
    }
    public void HardDrop(){
        _animator.CrossFadeInFixedTime(_HardDropSettings);
    }

    public void ApplyRootMotion(){
        _animator.applyRootMotion = true;
    }

    public void OnSMBEvent(string eventName){
        restrictMovement = true;
        print(eventName);
        Vector3 position = Vector3.zero;
        print(transform.rotation);
        switch(eventName){
            case "Jump":
             position = new Vector3(ObstacleAvoider.transform.position.x - (ObstacleAvoider.GetComponent<BoxCollider>().size.z/2), ObstacleAvoider.transform.position.y + (ObstacleAvoider.GetComponent<BoxCollider>().bounds.size.y/2), ObstacleAvoider.transform.position.z);
             print(position.y);
             print(ObstacleAvoider.GetComponent<BoxCollider>().bounds.size.y);
            _animator.MatchTarget( position, transform.rotation, AvatarTarget.Root, new MatchTargetWeightMask(Vector3.one, 1), 0.45f, 0.67f);
        break;
        case "JumpToFreeHang":
            position = new Vector3(ObstacleAvoider.transform.position.x - (ObstacleAvoider.GetComponent<BoxCollider>().bounds.size.x), (ObstacleAvoider.transform.position.y + ObstacleAvoider.GetComponent<BoxCollider>().bounds.size.y + 0.1f) -  (_controller.center.y), ObstacleAvoider.transform.position.z);
            _animator.MatchTarget( position, transform.rotation, AvatarTarget.Body, new MatchTargetWeightMask(Vector3.one, 1), 0.45f, 1f);
            print(ObstacleAvoider.transform.position.y + (ObstacleAvoider.GetComponent<BoxCollider>().bounds.size.y/2));
            print(position.y);
        break;
        case "Drop":
             position = new Vector3(ObstacleAvoider.transform.position.x - (ObstacleAvoider.GetComponent<BoxCollider>().size.z/2), (ObstacleAvoider.GetComponent<BoxCollider>().bounds.size.y/2) + ObstacleAvoider.transform.position.y, ObstacleAvoider.transform.position.z);
             position.y = 0;
            _animator.MatchTarget( position, transform.rotation, AvatarTarget.Root, new MatchTargetWeightMask(Vector3.one, 1), 0f, 1f);
        break;
        case "HardDrop":
             position = new Vector3(ObstacleAvoider.transform.position.x - (ObstacleAvoider.GetComponent<BoxCollider>().size.z/2), (ObstacleAvoider.GetComponent<BoxCollider>().bounds.size.y/2) + ObstacleAvoider.transform.position.y, ObstacleAvoider.transform.position.z);
             position.y = 0;
            _animator.MatchTarget( position, transform.rotation, AvatarTarget.Root, new MatchTargetWeightMask(Vector3.one, 1), 0f, 1f);
        break;
        case "Slide":
        sliding = true;
        restrictMovement = false;
        _animator.applyRootMotion = false;
        StartSliding();
        break;
        case "Exit":
        if(sliding){
            sliding = false;
            StopSliding();
        }
        print("animation End");
        restrictMovement = false;
        _animator.applyRootMotion = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        StartCoroutine(run());
        restrictMovement = false;
        break;
        }
    }

     private void StartSliding(){
        //shrinking _controller to impprove sliding
        _controller.height /= controllerSlidingHeight;
        _controller.center = new Vector3(_controller.center.x, _controller.center.y/controllerSlidingHeight, _controller.center.z);
    }

    private void StopSliding(){
        //restore _controller to normal height
        _controller.height *= controllerSlidingHeight;
        _controller.center = new Vector3(_controller.center.x, _controller.center.y*controllerSlidingHeight, _controller.center.z);
    }

    IEnumerator run(){
        float duration = 2f;
        float normalizedTime = 0.0f;

        _animator.CrossFade(_RunSettings);
        while (normalizedTime < 1.0f)
        {
            float currentSpeed = Mathf.Lerp(speedInterpolator.Min, speedInterpolator.Max, (speedInterpolator.Max * normalizedTime) + speedInterpolator.Value);
            float currentBlendSpeed = Mathf.Lerp(AIAnimationBlender.Min, AIAnimationBlender.Max, (AIAnimationBlender.Max * normalizedTime) + AIAnimationBlender.Blend);

            AIAnimationBlender.Blend = currentBlendSpeed;
            speedInterpolator.Value = currentSpeed;

            _animator.SetFloat(AIAnimationBlender.BlendName, AIAnimationBlender.Blend);
            
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }


    void FixedUpdate()
    {
        if(startChasing){
            if(!chasing){
                restrictMovement = false;
                chasing = true;
                StartCoroutine(run());
            }
        }
        if(!restrictMovement)
        _controller.Move(Vector3.right * speedInterpolator.Value * Time.fixedDeltaTime);
    }

    

    private void initAnimationSettings(){
    }
}
