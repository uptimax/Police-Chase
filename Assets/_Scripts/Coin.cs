using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
private Animator coinAnim;
private GameManager gameManager;
   private AudioManager _audioManager;

private void Awake() {
    coinAnim = GetComponent<Animator>();
    gameManager = FindObjectOfType<GameManager>();
    _audioManager = FindObjectOfType<AudioManager>();
}

private void Start(){
    gameManager.coins.Add(GetComponent<Coin>());
}

private void OnEnable() {
    coinAnim.SetTrigger("Spawn");
}

 private void OnTriggerEnter(Collider other) {
    Debug.Log(other.gameObject.name);
    if(other.CompareTag("Player")){
        coinAnim.SetTrigger("Collected");
        // Destroy(gameObject, 3.0f);
        _audioManager.Coin();
        FindObjectOfType<GameManager>().GetCoin();
    }
 }
}

