using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PowerupCooldown{
    public float coolDownTime;
    public PowerupType type;

    public PowerupCooldown(PowerupType type, float coolDownTime){
        this.type = type;
        this.coolDownTime = coolDownTime;
    }
}

public class AnimationBlend{
    public string BlendName;
    public float Min;
    public float Max;
    public float SmoothSpeed;
    public float Blend;
    public bool ReduceBlend;
    public AnimationBlend(float min, float max, float smoothSpeed, string blendName, bool reduceBlend = false, float blend = 0){
        BlendName = blendName;
        Min = min;
        Max = max;
        SmoothSpeed = smoothSpeed;
        ReduceBlend = reduceBlend;
        Blend = blend;
    }
}

public class FloatInterpolator{
    public float Min;
    public float Max;
    public float SmoothSpeed;
    public float Value;
    public bool ShouldReduceValue;
    public FloatInterpolator(float min, float max, float smoothSpeed, bool shouldReduceValue = false, float defualtValue = 0){
        Min = min;
        Max = max;
        SmoothSpeed = smoothSpeed;
        ShouldReduceValue = shouldReduceValue;
        Value = defualtValue;
    }
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private AudioManager AudioManager;
    private GameManager gameManager;
    private CharacterController _capsule;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform playerPos;
    private Rigidbody rb;
    private Vector3 travelDirection, _matchTargetPosition;
    private Quaternion _matchTargetRotation;
    public float gravity = .5f, jumpForce = 20.0f;
    private float controllerSlidingHeight = 3.0f, speed = 3.5f, verticalVelocity;

    [Header("Animation settings")]
    public CrossFadeSettings _runSetting;
    public CrossFadeSettings _jumpSetting;
    public CrossFadeSettings _dropSetting;
    public CrossFadeSettings _DeathSetting;
    public CrossFadeSettings _SoftLandingSetting;
    public CrossFadeSettings _SlideSetting;
    private MovementState state;
    public bool gameStart, jumping, _dropping, _sliding;
    public bool climbing;
    private SMB_EventCurrator _eventCurrator;
    public float _dropCheckDistance;
    public LayerMask wallLayerMask;
    private MatchTargetWeightMask _weightMask = new MatchTargetWeightMask(Vector3.one, 1);
    private bool rootMotionEnabled;
   public bool disbaleGravity, freeze, unlimited = true, restricted, disableJump;
   private AnimationBlend RunAnimationBlend;
   private FloatInterpolator SpeedInterpolator;
   private AudioManager _audioManager;

   private bool powerActive;
   public List<PowerupCooldown> powerupController  = new List<PowerupCooldown>();


    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _animator = GetComponent<Animator>();
        _animator.applyRootMotion = false;
        _capsule = GetComponent<CharacterController>();
        AudioManager = FindObjectOfType<AudioManager>();
        _eventCurrator = GetComponent<SMB_EventCurrator>();
        _eventCurrator.Event.AddListener(OnSMBEvent);
        _capsule.skinWidth = 0.0001f;
        travelDirection = Vector3.right;
       powerupController.Add(new PowerupCooldown(coolDownTime: 0, type: PowerupType.Magnet));
       powerupController.Add(new PowerupCooldown(coolDownTime: 0, type: PowerupType.SpeedBoost));
        
    }

    void FixedUpdate()
    {
        gameManager = FindObjectOfType<GameManager>();
        foreach(PowerupCooldown powerupcooldown in powerupController){
            switch(powerupcooldown.type){
                case PowerupType.Magnet:

                 gameManager.coins.ForEach((Coin coin)=>{
                  float distance = Vector3.Distance(transform.position, coin.gameObject.transform.TransformPoint(Vector3.zero));
                  if((int)distance == 4 && powerupcooldown.coolDownTime > 0){
                    coin.transform.parent.GetComponent<Magnet>().enableMagnetism(true);
                  }
                 });

                break;
                case PowerupType.SpeedBoost:
                break;
            }
            powerupcooldown.coolDownTime = Mathf.Clamp(powerupcooldown.coolDownTime - Time.deltaTime, 0, 20);
        }

        GameState gameState = gameManager.gameState;

        if(!(gameState == GameState.GameStart)) return;
        if(!_capsule.enabled) return;
        if(restricted) return;
        if(!gameStart){
            RunAnimationBlend = new AnimationBlend(blendName: "Run Blend", min: 0f, max: 1f, smoothSpeed: 0.005f, blend: 0.25f);
            SpeedInterpolator = new FloatInterpolator(min: 0f, max: speed, smoothSpeed: 0.05f, defualtValue: speed*0.25f);
            _animator.CrossFade("Blend Tree", 0);
            gameStart = true;
        }

        dropChecker();

        bool isGrounded = IsGrounded();
        bool isNearWall = IsNearWall();

        if(isGrounded)
        Run();
        Vector3 moveVector = Vector3.zero;

        if(isGrounded){
            verticalVelocity = -0.5f;
            if(MobileInput.Instance.SwipeUp && isGrounded && !disableJump && !jumping){
                 verticalVelocity = jumpForce;
                 jumping = true;
                 _audioManager.PlayerJump();
                _animator.CrossFadeInFixedTime(_jumpSetting);
            }

            //On player swipe down
            if(MobileInput.Instance.SwipeDown && isGrounded && !_sliding){
                 _sliding = true;
                _animator.CrossFadeInFixedTime(_SlideSetting);
                 StartSliding();
            }

        }else{
            verticalVelocity -= gravity;
        }

        moveVector = ApplyGravity(moveVector, verticalVelocity);
        if(isNearWall){
        moveVector = ApplyVelocity(moveVector, getInterpolatedSpeed());
        }

        //readjusting player z position back to zero to coushion root motion effect on player z axis
        Move(moveVector);
    }

    public void addPowerup(Powerup powerup){
       PowerupCooldown powerupCooldown = powerupController.Find((PowerupCooldown powerupCooldown)=> powerupCooldown.type == powerup.type);
       if(powerupCooldown != null){
        powerupCooldown.coolDownTime += powerup.duration;
        return;
       }

    }

     void Run(){
        if(RunAnimationBlend == null) return;
        _animator.SetFloat(RunAnimationBlend.BlendName , Mathf.Lerp(RunAnimationBlend.Min, RunAnimationBlend.Max, RunAnimationBlend.Blend));
        if(RunAnimationBlend.ReduceBlend){
         RunAnimationBlend.Blend -= RunAnimationBlend.SmoothSpeed;
         return;
        }
         RunAnimationBlend.Blend += RunAnimationBlend.SmoothSpeed;
    }

    private float getInterpolatedSpeed(){
        if(SpeedInterpolator == null) return 0f;
        float value = Mathf.Lerp(SpeedInterpolator.Min, SpeedInterpolator.Max, SpeedInterpolator.Value);
        if(SpeedInterpolator.ShouldReduceValue){
            SpeedInterpolator.Value -= SpeedInterpolator.SmoothSpeed;
        }else{
         SpeedInterpolator.Value += SpeedInterpolator.SmoothSpeed;
        }
        return value;
    }


    public void dropChecker(){
        if(true){
            if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit _hittInfo, _dropCheckDistance)){
                 Debug.DrawRay(transform.position, Vector3.down, Color.blue, _dropCheckDistance);
                 float groundDistance = _hittInfo.distance;
                //  print(!IsGrounded() && !jumping && !climbing && !_dropping);
                 if(!IsGrounded() && !jumping && !climbing && !_dropping && !_sliding){
                    _dropping = true;
                    if(groundDistance > 1.5f){
                     DisableRootMotion();
                    _animator.CrossFadeInFixedTime(_dropSetting);
                    }else if(groundDistance > 0.8f){
                        _animator.CrossFadeInFixedTime(_SoftLandingSetting);
                    }else{
                        _dropping = false;
                    }
                 }
            } 
        }
    }

    public void Move(Vector3 newPosition){
       if(freeze) return;
       _capsule.Move(newPosition * Time.fixedDeltaTime);
    }

    public Vector3 ApplyGravity(Vector3 targetPosition, float gravity){
        if(!disbaleGravity)
        targetPosition.y = gravity;

        return targetPosition;
    }
    public Vector3 ApplyVelocity(Vector3 targetPosition, float velocity){
        // if(!freeze)
        targetPosition.x = velocity;
        return targetPosition;
    }

    private bool IsGrounded(){
       var controllerBounds = _capsule.bounds;
       Ray groundRay = new Ray(new Vector3(
       controllerBounds.center.x,
        (controllerBounds.center.y - controllerBounds.extents.y) + 0.2f,
       controllerBounds.center.z),
        Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);
        return Physics.Raycast(groundRay, 0.2f + 0.095f);
    }

    private bool IsNearWall(){
       var controllerBounds = _capsule.bounds;
       Ray groundRay = new Ray(new Vector3(
       (controllerBounds.center.x + controllerBounds.extents.x) - 0.5f,
        controllerBounds.center.y,
        controllerBounds.center.z),
        Vector3.right);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red, 0.5f);
        return Physics.Raycast(groundRay, 0.05f + 0.6f);
    }

    private void StartSliding(){
        //shrinking _capsule to impprove sliding
        _capsule.height /= controllerSlidingHeight;
        _capsule.center = new Vector3(_capsule.center.x, _capsule.center.y/controllerSlidingHeight, _capsule.center.z);
    }

    private void StopSliding(){
        //restore _capsule to normal height
        _capsule.height *= controllerSlidingHeight;
        _capsule.center = new Vector3(_capsule.center.x, _capsule.center.y*controllerSlidingHeight, _capsule.center.z);
    }

    public void StateSwitcher(MovementState newState){
        state = newState;
    }

    public void EnableFreeze ()=> freeze = true;
    public void DisableFreeze ()=> freeze = false;
    public bool GetFreeze ()=> freeze;
    public void EnableUnlimited ()=> unlimited = true;
    public void DisableUnlimited ()=> unlimited = false;
    public bool GetUnlimited ()=> unlimited;
    public void EnableGravity ()=> disbaleGravity = false;
    public void DisableGravity ()=> disbaleGravity = true;
    public bool GetGravity ()=> disbaleGravity;

    public void RestrictSwiping ()=> restricted = true;
    public void UnrestrictSwiping ()=> restricted = false;

     public void OnSMBEvent(string eventName){
        print("drop enter");
        switch(eventName){
            case "JumpEnter":
            break;
            case "JumpExit":
            print("jump exited");
            jumping = false;
            RunAnimationBlend.Blend = 0.7f;
            SpeedInterpolator.Value = speed*0.1f;
            break;
            case "DropEnter":
            print("drop enter animation");
            print(_matchTargetPosition);    
            print("drop enter");
            break;
            case "HardDrop":
            SpeedInterpolator.Value = 0.1f;
            SpeedInterpolator.SmoothSpeed = 0.005f;
            break;
            case "DropExit":
            _dropping = false;
            RunAnimationBlend.Blend = 0.5f;
            SpeedInterpolator.SmoothSpeed = 0.05f;
            break;
            case "SlideExit":
            StopSliding();
            _sliding = false;
            break;
            case "AnimationExit":
            print("animation exited successfully");
            _capsule.enabled = true;
            _animator.applyRootMotion = false;
            break;
        }
    }

    private void ApplyRootMotion()=> _animator.applyRootMotion = true;
    private void DisableRootMotion()=> _animator.applyRootMotion = false;
    private void EnableCapsule()=> _capsule.enabled = true;
    private void DisableCapsule()=> _capsule.enabled = false;

    private Bounds GetCapsuleBounds{get{return _capsule.bounds;}}
    private void Crash(){
    
        FindObjectOfType<GameManager>().OnDeath();
        ApplyRootMotion();
        DisableCapsule();
        _animator.CrossFade(_DeathSetting);
        _audioManager.End();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        // print(hit.collider.CompareTag("Death"));
        if(!climbing)
        if(hit.collider.CompareTag("Death")){
            Crash();
        }
    }

      public void disbaleBasicControls(){
        disableJump = true;
        climbing = true;
     }

     public void enableBasicControls(){
        disableJump = false;
        jumping = false;
        climbing = false;
     }

     public void pause(bool shouldPause){

        if(shouldPause){
        Time.timeScale = 0;
        restricted = true;
        }else{
        Time.timeScale = 1;
        restricted = false;

        }
     }
}
