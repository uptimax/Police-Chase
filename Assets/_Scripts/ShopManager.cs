using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private GameStorage _gameStorage;
    private ModalManager _modalManager;
    private void Start(){
        _gameStorage = FindObjectOfType<GameStorage>();
        _modalManager = FindObjectOfType<ModalManager>();
    }

    public void buyMagnet(int price){
        int coin = _gameStorage.getCoin();
        if( coin > price){
            int newMagnet = _gameStorage.getMagnet() + 1;
            int newCoin = coin - price;
            _gameStorage.saveCoin(newCoin);
            _gameStorage.saveMagnet(newMagnet);

            this.updateUITextOfType("magnettext", newMagnet.ToString());
            this.updateUITextOfType("cointext", newCoin.ToString());
            _modalManager.assertPurchaseSuccessful();
            return;
        }

        //log insufficient coins
        _modalManager.assertInsufficientCoins();
        
    }

    public void buySpeedBoost(int price){
        int coin = _gameStorage.getCoin();
        if( coin > price){
            int newSpeedBoost = _gameStorage.getSpeedBoost() + 1;
            int newCoin = coin - price;
            _gameStorage.saveCoin(newCoin);
            _gameStorage.saveSpeedBoost(newSpeedBoost);

            this.updateUITextOfType("speedboosttext", newSpeedBoost.ToString());
            this.updateUITextOfType("cointext", newCoin.ToString());
            _modalManager.assertPurchaseSuccessful();
            return;
        }
        

        //log insufficient coins
        _modalManager.assertInsufficientCoins();
    }

    public void buy(ShopItem shopItem){
        switch(shopItem.type){
            case "magnet": 
            buyMagnet(shopItem.price);
            break;
            case "speedboost": 
            buySpeedBoost(shopItem.price);
            break;
        }
    }

}
