using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour

    // this script is going to be used on the coins themselves, and called from the collision part of the playercontroller 
{
    public float speed = 5f; // the speed at which the coin moves
    public float magnetDuration = 10;  // this is how long the powrup will last
    public bool ismagnetized = false;
    public Vector3 initialPos;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        initialPos = transform.position;
    }

    private void onEnable(){
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      if (ismagnetized) 
        {
            PowerUpMagnet();
        }
     
    }

    public void PowerUpMagnet()
    {
        MagnetCooldown();
    }

    private  void MagnetCooldown()
    // here's the small code for the magnet
    {
        Debug.Log("Magnet powerup activated");
        Vector3 targetPosition = playerTransform.position;
        targetPosition.y += .8f;
        if(!reseting)
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (speed * 2) * Time.deltaTime);
    }

    bool reseting;
    private void OnTriggerEnter(Collider other) {
    if(other.CompareTag("Player") && !reseting){
    reseting = true;
    Invoke("resetMagnetism", 1);
    print("reset coin");
    }
    }

    private void resetMagnetism(){
        transform.position = initialPos;
        ismagnetized = false;
        reseting = false;
    }

    public void enableMagnetism(bool shouldMagnetize){
        if(!ismagnetized)
        ismagnetized = shouldMagnetize;
    }
}
