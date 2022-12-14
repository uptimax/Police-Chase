using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStorage : MonoBehaviour
{
    private string coinKey = "coinKey";
    private string highscoreKey = "highscoreKey";
    private string magnetKey = "magnetKey";
    private string SpeedBoostKey = "SpeedBoostKey";

    public int getCoin()=> PlayerPrefs.GetInt(coinKey, 0);
    public void saveCoin(int value)=> PlayerPrefs.SetInt(coinKey, value);

    public int getHighScore()=> PlayerPrefs.GetInt(highscoreKey, 0);
    public void saveHighScore(int value)=> PlayerPrefs.SetInt(highscoreKey, value);

    public int getMagnet()=> PlayerPrefs.GetInt(magnetKey, 0);
    public void saveMagnet(int value)=> PlayerPrefs.SetInt(magnetKey, value);
    public int getSpeedBoost()=> PlayerPrefs.GetInt(SpeedBoostKey, 0);
    public void saveSpeedBoost(int value)=> PlayerPrefs.SetInt(SpeedBoostKey, value);
}
