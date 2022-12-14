/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupSelection : MonoBehaviour
{
    [Header("Navigation Buttons")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [Header("Play/Buy Buttons")]
    [SerializeField] private Button play;
    [SerializeField] private Button buy;
    [SerializeField] private Text priceText;

    [Header("Option Attributes")]
    [SerializeField] private int[] powerupPrices;
    private int currentPowerup;

    [Header("Sound")]
    [SerializeField] private AudioClip purchase;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        currentPowerup = SaveManager.instance.currentPowerup;
        SelectCar(currentPowerup);
    }

    private void SelectCar(int _index)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == _index);

        //UpdateUI();
    }
    private void UpdateUI()
    {
        //If current character unlocked show the play button
        if (SaveManager.instance.charactersUnlocked[currentPowerup])
        {
            play.gameObject.SetActive(true);
            buy.gameObject.SetActive(false);
        }
        //If not show the buy button and set the price
        else
        {
            play.gameObject.SetActive(false);
            buy.gameObject.SetActive(true);
            priceText.text = powerupPrices[currentPowerup] + "$";
        }
    }

    private void Update()
    {
        //Check if we have enough money
        if (buy.gameObject.activeInHierarchy)
            buy.interactable = (SaveManager.instance.money >= powerupPrices[currentPowerup]);
    }

    public void ChangeCar(int _change)
    {
        currentPowerup += _change;

        if (currentPowerup > transform.childCount - 1)
            currentPowerup = 0;
        else if (currentPowerup < 0)
            currentPowerup = transform.childCount - 1;

        SaveManager.instance.currentPowerup = currentPowerup;
        SaveManager.instance.Save();
        SelectCar(currentPowerup);
    }
    public void BuyCar()
    {
        SaveManager.instance.money -= powerupPrices[currentPowerup];
        //SaveManager.instance.charactersUnlocked[currentPowerup] = true;
        SaveManager.instance.Save();
        source.PlayOneShot(purchase);
        //UpdateUI();
    }
}*/
