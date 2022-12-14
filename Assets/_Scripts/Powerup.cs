using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] public PowerupType type;
    public float chanceToSpawn;
    public float duration;

    bool reset;
    private void OnTriggerEnter(Collider collider){
            print("enabling powerup");
        if(collider.CompareTag("Player") && !reset){
            collider.GetComponent<PlayerController>().addPowerup(GetComponent<Powerup>());
            gameObject.SetActive(false);
            reset = true;
            Invoke("resetPowerup", 20);
            print("enabling powerup");
        }
    }

    private void resetPowerup(){
        reset = false;
        gameObject.SetActive(true);
    }

    private void OnEnable(){
        gameObject.SetActive(Random.Range(0.0f, 1.0f) > chanceToSpawn);
  }

  private void OnDisable() {
        gameObject.SetActive(false);
  }
}
