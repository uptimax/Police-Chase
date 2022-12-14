using System.Collections;
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

    [Header("Powerup Attributes")]
    [SerializeField] private int[] powerupPrices;
    private int currentPowerup;

    [Header("Sound")]
    [SerializeField] private AudioClip purchase;
    private AudioSource source;

    [Header("Powerup Count")]
    public TextMeshProUGUI powerupText;

    private void Start()
    {
        // source = GetComponent<AudioSource>();
        // currentPowerup = SaveManager.instance.currentPowerup;
        // SelectPowerup(currentPowerup);
    }

    private void SelectPowerup(int _index)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == _index);

        UpdateCoins();
    }

    private void UpdateCoins()
    {
        // SaveManager.instance.powerupBalance += 1;
        // SaveManager.instance.Save();
        // powerupText.text = SaveManager.instance.powerupBalance.ToString();
    }

    /*private void Update()
    {
        //Check if we have enough money
        if (buy.gameObject.activeInHierarchy)
            buy.interactable = (SaveManager.instance.money >= powerupPrices[currentPowerup]);
    }*/

    public void ChangePowerup(int _change)
    {
        currentPowerup = _change;

        // if (currentPowerup > transform.childCount - 1)
        //     currentPowerup = 0;
        // else if (currentPowerup < 0)
        //     currentPowerup = transform.childCount - 1;

        // SaveManager.instance.currentPowerup = currentPowerup;
        // SaveManager.instance.Save();
        // SelectPowerup(currentPowerup);
    }
    public void BuyPowerup()
    {
        ShopItem shopItem = null;

        foreach(ShopItem item in GameObject.FindObjectsOfType<ShopItem>()){
            if(item.gameObject.activeSelf){
                shopItem = item;
                break;
            }
        }

        FindObjectOfType<ShopManager>().buy(shopItem);
        // SaveManager.instance.money -= powerupPrices[currentPowerup];
        //SaveManager.instance.charactersUnlocked[currentPowerup] = true;
        // SaveManager.instance.Save();
        // source.PlayOneShot(purchase);
        UpdateCoins();
    }
}
