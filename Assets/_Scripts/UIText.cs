using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    public string name;
    private TextMeshProUGUI text;

    private void Awake(){
        text = GetComponent<TextMeshProUGUI>();
    }
    public void updateText(string newText) => text.text = newText;
}
