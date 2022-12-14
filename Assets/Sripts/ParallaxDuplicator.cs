using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxDuplicator : MonoBehaviour
{
    private float sizeX;
    private float boundSizeX;
    private GameObject background;
    public GameObject target;
    private void Awake() {
        background = GameObject.FindWithTag("Background");
        sizeX = background.transform.localScale.x;
        boundSizeX = background.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        GameObject duplicate = Instantiate(target, new Vector3((boundSizeX*sizeX) * 3, transform.position.y, transform.position.z), transform.rotation);
        print(this.name);
        duplicate.transform.parent = transform.parent;
    }
    void Update()
    {
        
    }
}
