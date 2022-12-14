using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// [RequireComponent(typeof(BoxCollider))]
public class parallax : MonoBehaviour
{
    // private float length, startpos;
    // private Vector3 startPos;
    // private Transform cam;
    // public float parallaxEffect;
    // private float repeatWidth;


    // Start is called before the first frame update
    // void Start()
    // {
    //     cam = Camera.main.transform;
    //     startPos = transform.position;
    //     length = GetComponent<SpriteRenderer>().bounds.size.x;
    //     repeatWidth = GetComponent<BoxCollider>().size.x;
    // }

    // Update is called once per frame
    // void Update()
    // {
    //     float temp = ((cam.position.x * (1 - parallaxEffect)));
    //     float dist = (cam.position.x * parallaxEffect);

    //     transform.position = new Vector3(startpos * dist, transform.position.y, transform.position.z);

    //     if (transform.position.x < startPos.x - repeatWidth)
    //     {
    //         transform.position = startPos;
    //     }
    //     if (temp > startpos + length) startpos += length;
    //     else if (temp < startpos - length) startpos -= length;
    // }

    // [Header("Layer Setting")]
    // public float Layer_Speed;
    // private Transform _camera;
    // private float startPos;
    // private float boundSizeX;
    // private float sizeX;
    // private GameObject Layer_0;
    // private GameObject background;
    // void Start()
    // {
    //     background = GameObject.FindWithTag("Background");
    //     _camera = Camera.main.transform;
    //     sizeX = background.transform.localScale.x;
    //     boundSizeX = background.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        
    //         startPos = transform.position.x;
    //         print(transform.position.x);
        
    // }

    // void FixedUpdate(){
    //         float temp = (_camera.position.x * (1-Layer_Speed) );
    //         float distance = _camera.position.x  * Layer_Speed;

    //         transform.position =  Vector3.Lerp(transform.position, new Vector3(startPos + distance, transform.position.y, transform.position.z), 0.1f);

    //         if (temp > startPos + boundSizeX*sizeX){
    //             startPos += (boundSizeX*sizeX) * 3;
    //         }else if(temp < startPos - ((boundSizeX*sizeX) * 3)){
    //             startPos -= (boundSizeX*sizeX) * 3;
    //         }
            
    //     }

    public float multiplier;
    public bool horizontalOnly;
    public bool calculateInfiniteHorizontalPosition;
    public bool calculateInfiniteVerticalPosition;
    public bool isInfinite;
    public bool isLastParallaxBackground;

    private Vector3 startCameraPosition;
    private float length;
    private float endPosition;
    private Camera _camera;
    private Vector3 startPosition;
    public GameObject lastParallaxBackground;
    void Start(){
        _camera = Camera.main;
        startPosition = transform.position;
        if(isInfinite)
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        CalculateStartPosition();
    }

    void Awake(){
        // if(isLastParallaxBackground){
        // endPosition = transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x;
        // }else{
        // endPosition = lastParallaxBackground.transform.position.x + lastParallaxBackground.GetComponent<SpriteRenderer>().bounds.size.x;
        // }

        // print(endPosition);
    }

    void CalculateStartPosition(){
        float distX = (_camera.transform.position.x - transform.position.x) * multiplier;
        float distY = (_camera.transform.position.y - transform.position.y) * multiplier;
        Vector3 tmp = new Vector3(startPosition.x, startPosition.y);

        if(calculateInfiniteHorizontalPosition)
            tmp.x = transform.position.x + distX;
        if(calculateInfiniteVerticalPosition)
            tmp.y = transform.position.y + distY;

        startPosition = tmp;
    }

    void FixedUpdate(){
        Vector3 position = startPosition;

        if(isInfinite){
            float tmp = _camera.transform.position.x * (1 - multiplier);

        if(horizontalOnly)
        position.x += multiplier * (_camera.transform.position.x - startCameraPosition.x);
        else
            position += multiplier * (_camera.transform.position - startCameraPosition);
        position.z = transform.position.z;
        transform.position = position;

            if(tmp > startPosition.x + length){
                if(isLastParallaxBackground){
                startPosition.x += GetComponent<SpriteRenderer>().bounds.size.x * 2;
                }else{
                startPosition.x += GetComponent<SpriteRenderer>().bounds.size.x * 2;
                // startPosition.x += endPosition;
                }
            }
            // else if(tmp < startPosition.x - length)
            //     startPosition.x -= length;
        }
        }

    }

