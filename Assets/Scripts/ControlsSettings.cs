using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsSettings : MonoBehaviour
{
    private int stateControls = 2; // 0->Space 1->Click 2->Mouse Over (Default)

    public GameObject buttonSelectorViewer;

    public Transform[] differentPos;

    public TextMeshProUGUI textToChange;

    void Start()
    {
        buttonSelectorViewer.transform.position = differentPos[stateControls].position;
    }


    void Update()
    {
        
    }

    public void ClickOnSelector()
    {
        if (stateControls == 0)
        {
            stateControls = 1;
            textToChange.text = "Click";
        }
        else if(stateControls == 1)
        {
            stateControls = 2;
            textToChange.text = "Mouse Over";
        }
        else
        {
            stateControls = 0;
            textToChange.text = "Space";
        }
        buttonSelectorViewer.transform.position = differentPos[stateControls].position;


    }
}
