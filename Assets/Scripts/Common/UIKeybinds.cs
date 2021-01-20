using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIKeybinds : MonoBehaviour
{
    public List<GameObject> buttons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupKeybinds(List<Tuple<string, string>> keybindActions)
    {
        if (keybindActions.Count > 4)
        {
            Debug.Log("TOO MANY KEYBINDS!");
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < keybindActions.Count) {
                buttons[i].SetActive(true);
                buttons[i].gameObject.transform.Find("KeybindLetter").GetComponent<TextMeshProUGUI>().text = keybindActions[i].Item1;
                buttons[i].gameObject.transform.Find("KeybindAction").GetComponent<TextMeshProUGUI>().text = keybindActions[i].Item2;
            }
            else
            {
                buttons[i].SetActive(false);
            }
        }
    }
}
