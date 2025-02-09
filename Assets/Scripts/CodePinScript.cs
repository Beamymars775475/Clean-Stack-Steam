using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodePinScript : MonoBehaviour
{
    private string digitsPressedTotal; // ORDRE DES TRUCS QUI S'ILLUMINE

    private TextMeshProUGUI screenText;

    public LevelSelectorScript levelSelectorScript;

    void Start()
    {
        digitsPressedTotal = "";

        screenText = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        RefreshUIScreen();
    }

    public void PressDigit(int numberOfDigit)
    {
        if(digitsPressedTotal.Length > 3)
        {
            digitsPressedTotal = "";
        }
        digitsPressedTotal += numberOfDigit;

        RefreshUIScreen();
    }

    public void RefreshUIScreen()
    {
        screenText.text = "";
        foreach(char number in digitsPressedTotal)
        {
            screenText.text += number;
        }
        for(int i = 0; i < 4-digitsPressedTotal.Length; i++)
        {
            screenText.text += "-";
        }
    }

    public void PressYes()
    {
        if(digitsPressedTotal == "0000") // PLACEHOLDER
        {
            levelSelectorScript.GoDownOrUp();
            StateButtons();
        }
        if(digitsPressedTotal == "1234") // PLACEHOLDER
        {
            levelSelectorScript.GoDownOrUpDeeper();
            StateButtons();
        }
    }

    public void PressNo()
    {
        digitsPressedTotal = "";
        RefreshUIScreen();
    }

    public void StateButtons()
    {
        if(levelSelectorScript.GetStateOfLevel() != 1)
        {
            foreach(Transform button in gameObject.transform.GetChild(0))
            {
                Button buttoncomp = button.GetComponent<Button>();
                if(buttoncomp != null)
                {
                    buttoncomp.interactable = false;
                }
            }
        }
        else
        {
            foreach(Transform button in gameObject.transform.GetChild(0))
            {
                Button buttoncomp = button.GetComponent<Button>();
                if(buttoncomp != null)
                {
                    buttoncomp.interactable = true;
                }
            }
        }
    }
}
