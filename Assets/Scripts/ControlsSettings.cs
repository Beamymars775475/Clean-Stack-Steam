using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsSettings : MonoBehaviour, IDataPersistence
{
    [Header("Controls")]

    public GameObject buttonSelectorViewerControlPreferences;
    public Transform[] differentPos1;

    public TextMeshProUGUI textToChangeControlPreferences;

    [Header("Inventory Apparence")]

    public GameObject buttonSelectorViewerInv;

    public Transform[] differentPos2;

    public TextMeshProUGUI textToChangeInv;

    public bool isNeedToInvisible;

    public bool modeHard;

    public int controlsPref;  // 0->Space 1->Click 2->Mouse Over (Default)

    


    public void ClickOnSelectorControls() // Bouton du haut
    {
        if (controlsPref == 0)
        {
            controlsPref = 1;
            GameManager.instance.controlsPreference = controlsPref;
            textToChangeControlPreferences.text = "Click";
        }
        else if(controlsPref == 1)
        {
            controlsPref = 2;
            GameManager.instance.controlsPreference = controlsPref;
            textToChangeControlPreferences.text = "MouseOver";
        }
        else
        {
            controlsPref = 0;
            GameManager.instance.controlsPreference = controlsPref;
            textToChangeControlPreferences.text = "Space";
        }
        buttonSelectorViewerControlPreferences.transform.position = differentPos1[controlsPref].position;
    }

    public void ClickOnSelectorInvTransparency() // Bouton du bas
    {
        if (isNeedToInvisible == true)
        {
            isNeedToInvisible = false;
            textToChangeInv.text = "Off";
            buttonSelectorViewerInv.transform.position = differentPos2[1].position;
        }
        else
        {
            isNeedToInvisible = true;
            textToChangeInv.text = "On";
            buttonSelectorViewerInv.transform.position = differentPos2[0].position;
        }
        GameManager.instance.isTransparencyNeeded = isNeedToInvisible;
    }

    public void ClickOnDeleteData()
    {
        DataPersistenceManager.instance.ResetGame();
    }

    public void LoadData(GameData data)
    {
        modeHard = data.isHardMode;
        // need to work on it


        controlsPref = data.controlsChoice;
        if (controlsPref == 0)
        {
            textToChangeControlPreferences.text = "Space";
        }
        else if(controlsPref == 1)
        {
            textToChangeControlPreferences.text = "Click";
        }
        else
        {
            textToChangeControlPreferences.text = "MouseOver";
        }
        buttonSelectorViewerControlPreferences.transform.position = differentPos1[controlsPref].position;


        isNeedToInvisible = data.isInvTransparent;
        if (isNeedToInvisible == true)
        {
            textToChangeInv.text = "On";
            buttonSelectorViewerInv.transform.position = differentPos2[0].position;
        }
        else
        {
            textToChangeInv.text = "Off";
            buttonSelectorViewerInv.transform.position = differentPos2[1].position;
        }
    }

    public void SaveData(GameData data)
    {
        data.isHardMode = modeHard;




        data.controlsChoice = controlsPref;
        if (controlsPref == 0)
        {
            textToChangeControlPreferences.text = "Space";
        }
        else if(controlsPref == 1)
        {
            textToChangeControlPreferences.text = "Click";
        }
        else
        {
            textToChangeControlPreferences.text = "MouseOver";
        }




        data.isInvTransparent = isNeedToInvisible;
        if (isNeedToInvisible == true)
        {
            textToChangeInv.text = "On";

        }
        else
        {
            textToChangeInv.text = "Off";

        }
    } 
}
