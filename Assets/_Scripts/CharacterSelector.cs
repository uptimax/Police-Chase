using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] characters;
    private int selectedCharacter = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject character in characters){
            character.SetActive(false);
        }

        characters[selectedCharacter].SetActive(true);
        
    }

    public void SelectCharacter(int newCharacterIndex){

        characters[selectedCharacter].SetActive(false);
        characters[newCharacterIndex].SetActive(true);
        selectedCharacter = newCharacterIndex;
    }
}
