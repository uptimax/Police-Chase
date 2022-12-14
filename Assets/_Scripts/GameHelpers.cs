using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelpers 
{
     public static void updateUITextOfType(this MonoBehaviour behaviour, string type, string newText){
       foreach(UIText uiText in GameObject.FindObjectsOfType<UIText>()){
        if(uiText.name == type)
        uiText.updateText(newText);
       }
    }
}
