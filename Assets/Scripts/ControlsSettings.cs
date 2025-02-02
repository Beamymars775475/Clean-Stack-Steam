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

    public GameObject SecurityPanel;

    

    void Start()
    {
        SecurityPanel.SetActive(false);
    }

    
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

    public void ClickOnDeleteDataButton()
    {
        SecurityPanel.SetActive(true);
    }

    public void ClickYesDeleteData()
    {
        DataPersistenceManager.instance.ResetDataLevels();
        SecurityPanel.SetActive(false);
    }

    public void ClickNoDeleteData()
    {
        SecurityPanel.SetActive(false);
    }


    public void ClickOnDeleteParameters()
    {
        DataPersistenceManager.instance.DefaultParameters();
    }

    public void LoadData(GameData data)
    {
        modeHard = data.isHardMode;
        // need to work on it


        controlsPref = data.controlsChoice;
        GameManager.instance.controlsPreference = data.controlsChoice;
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
    } 
}
