/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyPowerup : MonoBehaviour, IPointerClickHandler
{

    [Header("Powerup Attributes")]
    [SerializeField] private int[] powerupPrices;
    private int currentPowerup;

    [Header("Sound")]
    [SerializeField] private AudioClip purchase;
    private AudioSource source;

    [SerializeField] private Text priceText;

    /*private void UpdateCoins()
    {
        priceText.text = powerupPrices[currentPowerup] + "$";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SaveManager.instance.money -= powerupPrices[currentPowerup];
        SaveManager.instance.Save();
        source.PlayOneShot(purchase);

        UpdateCoins();
    }*/

    /*void Start()
    {
       switchArea = getSwitchArea.GetComponent<SwitchArea>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        switchArea = getSwitchArea.GetComponent<SwitchArea>();
        switchArea.BuyThePowerup();
        Debug.Log("You bought this powerup");
    }
}*/
