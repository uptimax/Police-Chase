using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    private CharacterController controller;
    private PlayerController playerController;
    private CharacterController _capsule;

    [Header("Climb Settings")]
    [SerializeField] private float _wallAngleMax, _groundAngleMax;
    [SerializeField] private LayerMask _layerMaskClimb;

    [Header("Heights")]
    [SerializeField] private float _overpassHeight, _hangHeight, _climbUpHeight, _vaultHeight, _stepHeight;

    [Header("Offset")]
    [SerializeField] private Vector3 _endOffset, _hangOffset, _climbOrigindown;
    [Header("Animation Settings")]
    public CrossFadeSettings _standToFreeHandSetting, _hangToClimbEnterSetting, _hangToClimbExitSetting, _climbUpSetting, _vaultSetting, _stepUpSetting, _dropSetting, _dropToAirSetting;

    private Vector3 _endPosition, _matchTargetPosition;
    private Quaternion _matchTargetRotation, _forwardNormalXZRotation, _animationTargetRotation;

    private RaycastHit _downRaycastHit, _forwardRaycastHit;
    private MatchTargetWeightMask _weightMask = new MatchTargetWeightMask(Vector3.one, 1);

    [SerializeField] private Animator _animator;
    private SMB_EventCurrator _eventCurrator;
    private float _climbFowardDistance;
    private bool _climbing, _allowAutomaticClimb;

    private void Start() {
         controller = GetComponent<CharacterController>();
        _capsule = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();

        _animator = GetComponent<Animator>();
        _eventCurrator = GetComponent<SMB_EventCurrator>();
        _eventCurrator.Event.AddListener(OnSMBEvent);
        _animator.applyRootMotion = false;

    }


    /// Callback for processing animation movements for modifying root motion.
    
    private void FixedUpdate() {
        var _capsuleBounds = _capsule.bounds;
        Ray forwardRay = new Ray(new Vector3((_capsuleBounds.center.x + _capsuleBounds.extents.x), _capsuleBounds.center.y/2, _capsuleBounds.center.z) , Vector3.right);
        Debug.DrawRay(forwardRay.origin, forwardRay.direction, Color.white, 4f);
        

        if(!_climbing){
            if(MobileInput.Instance.SwipeUp){
                if(Physics.Raycast(forwardRay, out RaycastHit _hittInfo, 3f, _layerMaskClimb) && !_allowAutomaticClimb){
                playerController.disbaleBasicControls();
                 float targetDistance = _hittInfo.distance;
                 _allowAutomaticClimb = true;
                 _climbFowardDistance = (transform.position.x + targetDistance ) - _climbOrigindown.z;
                }
            }

            
             if(_allowAutomaticClimb){
                // print("enabling automatic jump");
                if(_climbFowardDistance <= transform.position.x)
                if(CanClimb(out _downRaycastHit, out _forwardRaycastHit, out _endPosition)){
                    InitaiteClimb();
             }
             }
        }
    }

    private void OnAnimatorMove(){
        if(_animator.isMatchingTarget)
         _animator.ApplyBuiltinRootMotion();
    }

    private bool CanClimb(out RaycastHit downRaycastHit, out RaycastHit forwardRaycastHit, out Vector3 endPosition){
        endPosition = Vector3.zero;
        downRaycastHit = new RaycastHit();
        forwardRaycastHit = new RaycastHit();

        bool _downHit;
        bool _forwardHit;
        bool _overpassHit;
        float _climbHeight;
        float _groundAngle;
        float _wallAngle;

        RaycastHit _downRaycastHit;
        RaycastHit _forwardRaycastHit;
        RaycastHit _overpassRaycastHit;

        Vector3 _endPosition;
        Vector3 _forwardDirectionXZ;
        Vector3 _forwardNormalXZ;

        Vector3 _downDirection = Vector3.down;
        Vector3 _downOrigin = transform.TransformPoint(_climbOrigindown);
        _downHit = Physics.Raycast(_downOrigin, _downDirection, out _downRaycastHit, _climbOrigindown.y - _stepHeight, _layerMaskClimb);
              Debug.DrawRay(_downOrigin, _downDirection, Color.yellow, 0.5f);
        print(_downHit);
        if(_downHit){
        // print("climbing");
            //forward + overpass cast 
            float _forwardDistance = _climbOrigindown.z;
            Vector3 _forwardOrigin = new Vector3(transform.position.x, _downRaycastHit.point.y - 0.1f, transform.position.z);
            Vector3 _overpassOrigin = new Vector3(transform.position.x, _overpassHeight, transform.position.z);

            _forwardDirectionXZ = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            _forwardHit = Physics.Raycast(_forwardOrigin, _forwardDirectionXZ, out _forwardRaycastHit, _forwardDistance, _layerMaskClimb);
            _overpassHit = Physics.Raycast(_overpassOrigin, _forwardDirectionXZ, out _overpassRaycastHit, _forwardDistance, _layerMaskClimb);
              Debug.DrawRay(_forwardOrigin, _forwardDirectionXZ, Color.green, 0.5f);
        print(_forwardHit);
            _climbHeight = _downRaycastHit.point.y - transform.position.y;
            //  print(_forwardHit);
            //  print(_overpassHit || _climbHeight < _overpassHeight);
            if(_forwardHit)
                if(_overpassHit || _climbHeight < _overpassHeight){
                    //Angles
                    _forwardNormalXZ = Vector3.ProjectOnPlane(_forwardRaycastHit.normal, Vector3.up);
                    _groundAngle = Vector3.Angle(_downRaycastHit.normal, Vector3.up);
                    _wallAngle = Vector3.Angle(_forwardNormalXZ, _forwardDirectionXZ);
                    print(_wallAngle <= _wallAngleMax);
                    print(_wallAngle);
                    // print(_wallAngleMax);
                    if(_wallAngle <= _wallAngleMax)
                        if(_groundAngle <= _groundAngleMax){
                            //End offset
                            Vector3 _vectSurface = Vector3.ProjectOnPlane(_forwardDirectionXZ, _downRaycastHit.normal);
                            _endPosition = _downRaycastHit.point + Quaternion.LookRotation(_vectSurface, Vector3.up) * _endOffset;

                            //De-penetration
                            Collider _colliderB = _downRaycastHit.collider;
                            bool _penetrationOverlap = Physics.ComputePenetration(
                                colliderA: _capsule,
                                positionA: _endPosition,
                                rotationA: transform.rotation,
                                colliderB: _colliderB,
                                positionB: _colliderB.transform.position,
                                rotationB: _colliderB.transform.rotation,
                                direction: out Vector3 _penetrationDirection,
                                distance: out float _penetrationDistance);
                                if(_penetrationOverlap)
                                    _endPosition += _penetrationDirection * _penetrationDistance;

                                    //Up Sweep
                                    float _inflate = -0.05f;
                                    float _upSweepDistance = _downRaycastHit.point.y - transform.position.y;
                                    Vector3 _upSweepDirection = transform.up;
                                    Vector3 _upSweepOrigin = transform.position;
                                    bool _upSweepHit = CharacterSweep(
                                        position: _upSweepOrigin,
                                        rotation: transform.rotation,
                                        direction: _upSweepDirection,
                                        distance: _upSweepDistance,
                                        layerMask: _layerMaskClimb,
                                        inflate: _inflate
                                    );

                                    //forward Sweep
                                    Vector3 _forwardSweepOrigin = transform.position + _upSweepDirection * _upSweepDistance;
                                    Vector3 _forwardSweepVector = _endPosition - _forwardSweepOrigin;
                                    bool _forwardSweepHit = CharacterSweep(
                                        position: _forwardSweepOrigin,
                                        rotation: transform.rotation,
                                        direction: _forwardSweepVector.normalized,
                                        distance: _forwardSweepVector.magnitude,
                                        layerMask: _layerMaskClimb,
                                        inflate: _inflate
                                    );


                                    if(!_upSweepHit && !_forwardSweepHit){
                                        endPosition = _endPosition;
                                        downRaycastHit = _downRaycastHit;
                                        forwardRaycastHit = _forwardRaycastHit;
                                        return true;
                                    }
                                    
                                
                        }
                    
                }
            
        }
        return false;
    }

    private bool CharacterSweep(Vector3 position, Quaternion rotation, Vector3 direction, float distance, LayerMask layerMask, float inflate){
        //Assuming capsule is on y axis
        float _heightScale = Mathf.Abs(transform.lossyScale.y);
        float _radiusScale = Mathf.Max(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.z));

        float _radius = _capsule.radius * _radiusScale;
        float _totalHeight = Mathf.Max(_capsule.height * _heightScale, _radius * 2);

        Vector3 _capsuleUp = rotation * Vector3.up;
        Vector3 _center = position + rotation * _capsule.bounds.center;
        Vector3 _top = _center + _capsuleUp * (_totalHeight / 2 - _radius);
        Vector3 _bottom = _center - _capsuleUp * (_totalHeight / 2 - _radius);
        // print(distance);
        // print(_bottom);

        
        bool _sweepHit = Physics.CapsuleCast(
            point1: _bottom,
            point2: _top,
            radius: _radius + inflate,
            direction: direction,
            maxDistance: distance,
            layerMask: layerMask
        );

        return _sweepHit;
    }

    private void InitaiteClimb(){
        _climbing = true;
        playerController.disbaleBasicControls();
        _capsule.enabled = false;
        _animator.SetBool("Run", false);
        _animator.applyRootMotion = true;

        print("initiating");

        float _climbHeight = _downRaycastHit.point.y - transform.position.y;
        Vector3 _forwardNormalXZ = Vector3.ProjectOnPlane(_forwardRaycastHit.normal, Vector3.up);
        _forwardNormalXZRotation = Quaternion.LookRotation(_forwardNormalXZ, Vector3.up);
       

        if(_climbHeight > _hangHeight){
            _matchTargetPosition = _forwardRaycastHit.point + _forwardNormalXZRotation * _hangOffset;
            _matchTargetPosition.z = transform.position.z;
            _matchTargetRotation = _forwardNormalXZRotation;
            _animator.CrossFade(_standToFreeHandSetting);
        }else if(_climbHeight > _climbUpHeight){
            _matchTargetPosition = _endPosition;
            _matchTargetRotation = _forwardNormalXZRotation;
            print("runing this section");
            _animator.CrossFadeInFixedTime(_climbUpSetting);
        }else if(_climbHeight > _vaultHeight){
            _matchTargetPosition = _endPosition;
            _matchTargetRotation = _forwardNormalXZRotation;
            _animator.CrossFadeInFixedTime(_vaultSetting);
        }else if(_climbHeight > _stepHeight){
            _matchTargetPosition = _endPosition;
            _matchTargetRotation = _forwardNormalXZRotation;
            _animator.CrossFadeInFixedTime(_stepUpSetting);
        }else{
         _climbing = false;
        playerController.enableBasicControls();
        _capsule.enabled = true;
        _animator.SetBool("Run", true);
        _animator.applyRootMotion = false;
        }
    }

    public void OnSMBEvent(string eventName){
        _matchTargetRotation = transform.rotation;

        //this prevent the match target position from change the player original z position
        print(_matchTargetPosition.z);
        switch(eventName){
            case "StandToFreeHangEnter":
            _animator.MatchTarget(_matchTargetPosition, _matchTargetRotation, AvatarTarget.Root, _weightMask, 0f, 0.65f);
            break;
             case "StandToFreeHangExitEnter":
            _animator.MatchTarget(_matchTargetPosition, _matchTargetRotation, AvatarTarget.Root, _weightMask, 0.9f, 1f);
            break;
            case "ClimbUpEnter":
            print("climbUpEnter");
            _animator.MatchTarget(_matchTargetPosition, _matchTargetRotation, AvatarTarget.Root, _weightMask, 0f, 0.5f);
            break;
            case "HangToClimbUp":
            _animator.MatchTarget(_matchTargetPosition, _matchTargetRotation, AvatarTarget.Root, _weightMask, 0, 1f);
            break;
            case "VaultEnter":
            print("vault");
            _animator.MatchTarget(_matchTargetPosition, _matchTargetRotation, AvatarTarget.Root, _weightMask, 0, 1f);
            break;
            case "StepUpEnter":
            _animator.MatchTarget(_matchTargetPosition, _matchTargetRotation, AvatarTarget.Root, _weightMask, 0.3f, 0.8f);
            break;
            case "StandToFreeHangExit":
             _matchTargetPosition = _endPosition;
            _matchTargetRotation = _forwardNormalXZRotation;
            _animator.CrossFadeInFixedTime(_hangToClimbExitSetting);
            print("exiting");
            break;
            case "ClimbExit":
            break;
            case "VaultExit":
            break;
            case "StepUpExit":
            break;
            case "Exit":
            print("animation exited");
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            _allowAutomaticClimb = false;
            _climbing = false;
            _capsule.enabled = true;
            _animator.applyRootMotion = false;
            playerController.enableBasicControls();
            break;
        }
    }

   
   
}
