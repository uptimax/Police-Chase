using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FollowVector{
    public bool X;
    public bool Y;
    public bool Z;
    // public static FollowVector factory()=> new FollowVector();
}

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Vector3 offset;
    public float smoothSpeed;
    public float damping;
    private Vector3 velocity = Vector3.zero;
    private Vector2 velocity2D = Vector2.zero;
    public FollowVector followDirection;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - _target.position;        
    }

    private void FixedUpdate() {
        Vector3 movePosition = _target.position + offset;
        Vector3 position = transform.position;
        if(followDirection.X){
            transform.position = Vector3.SmoothDamp(position, new Vector3(movePosition.x, position.y, position.z), ref velocity, damping);
        }
        
        if(followDirection.Y){
            transform.position = Vector3.SmoothDamp(position, new Vector3(position.x, movePosition.y, position.z), ref velocity, damping);
        }
        
        if(followDirection.Z){
            transform.position = Vector3.SmoothDamp(position, new Vector3(position.x, position.y, movePosition.z), ref velocity, damping);
        }


    }
}
