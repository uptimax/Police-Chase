using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalManager : MonoBehaviour
{
    private Animator _animator;

    private void Start(){
        _animator = GetComponent<Animator>();
    }
    public void assertInsufficientCoins(){
        print("insufficient coins");
    }
    public void assertPurchaseSuccessful(){
        print("purchase successfull");
    }
}
