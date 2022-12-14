using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Vector3 offset;
    public float smoothSpeed;
    public float damping;
    public Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        print(gameObject.name);        
        offset = transform.position - target.position;
    }

    private void FixedUpdate() {
        Vector3 movePosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
    }

    IEnumerator EnterGamePlayPosition(){
        float duration = 1f;
        float normalizedTime = 0f;
        while (normalizedTime < 1f)
        {
            offset = new Vector3(2.7f, Mathf.Clamp(0f, 3.76f, 3.76f * normalizedTime), Mathf.Clamp(-8.41f, -8.41f,  -8.41f * normalizedTime));
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    public void start(){
        StartCoroutine(EnterGamePlayPosition());
    }
}
