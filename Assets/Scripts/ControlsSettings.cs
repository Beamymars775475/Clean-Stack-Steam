using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsSettings : MonoBehaviour, IDataPersistence
{
    [Header("Controls")]

    public GameObject buttonSelectorViewerControlPreferences;
    public Transform[] differentPos1;

    public TextMeshProUGUI textToChangeControlPreferences;

    [Header("Inventory Apparence")]

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
    }

    public void SaveData(GameData data)
    {

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
