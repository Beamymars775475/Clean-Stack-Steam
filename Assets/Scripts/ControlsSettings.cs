using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsSettings : MonoBehaviour
{
    [Header("Controls")]

    public GameObject buttonSelectorViewerControlPreferences;
    public Transform[] differentPos1;

    public TextMeshProUGUI textToChangeControlPreferences;

    [Header("Inventory Apparence")]

    public GameObject buttonSelectorViewerInv;

    public Transform[] differentPos2;

    public TextMeshProUGUI textToChangeInv;

    void Start()
    {
        buttonSelectorViewerControlPreferences.transform.position = differentPos1[GameManager.instance.controlsPreference].position;

        if(GameManager.instance.isTransparencyNeeded == true)
        {
            buttonSelectorViewerInv.transform.position = differentPos2[0].position;
            textToChangeInv.text = "On";
        }
        else
        {
            buttonSelectorViewerInv.transform.position = differentPos2[1].position;
            textToChangeInv.text = "Off";
        }


    }


    void Update()
    {
        
    }

    public void ClickOnSelectorControls() // Bouton du haut
    {
        if (GameManager.instance.controlsPreference == 0)
        {
            GameManager.instance.controlsPreference = 1;
            textToChangeControlPreferences.text = "Click";
        }
        else if(GameManager.instance.controlsPreference == 1)
        {
            GameManager.instance.controlsPreference = 2;
            textToChangeControlPreferences.text = "MouseOver";
        }
        else
        {
            GameManager.instance.controlsPreference = 0;
            textToChangeControlPreferences.text = "Space";
        }
        buttonSelectorViewerControlPreferences.transform.position = differentPos1[GameManager.instance.controlsPreference].position;
    }

    public void ClickOnSelectorInvTransparency() // Bouton du bas
    {
        if (GameManager.instance.isTransparencyNeeded == true)
        {
            GameManager.instance.isTransparencyNeeded = false;
            textToChangeInv.text = "Off";
        }
        else
        {
            GameManager.instance.isTransparencyNeeded = true;
            textToChangeInv.text = "On";
        }

        if(GameManager.instance.isTransparencyNeeded == true)
        {
            buttonSelectorViewerInv.transform.position = differentPos2[0].position;
        }
        else
        {
            buttonSelectorViewerInv.transform.position = differentPos2[1].position;
        }
    }
}
