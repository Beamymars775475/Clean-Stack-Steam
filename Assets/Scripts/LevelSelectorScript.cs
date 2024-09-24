using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorScript : MonoBehaviour
{

    public GameObject monde4;
    public GameObject monde5;
    public GameObject monde6;
    public GameObject monde7;

    void Start()
    {
        monde4.SetActive(false);
        monde5.SetActive(false);
        monde6.SetActive(false);
        monde7.SetActive(false);
    }

    void Update()
    {
        if(GameManager.instance.levelsState[11] == true)
        {
            monde4.SetActive(true);
        }

        if(GameManager.instance.levelsState[23] == true)
        {
            monde5.SetActive(true);
        }

        if(GameManager.instance.levelsState[35] == true)
        {
            monde6.SetActive(true);
        }

        if(GameManager.instance.levelsState[11] == true && GameManager.instance.levelsState[23] == true && GameManager.instance.levelsState[35] == true && GameManager.instance.levelsState[43] == true && GameManager.instance.levelsState[52] == true)
        {
            monde7.SetActive(true);
        }

    }


    void ClickOnLevel()
    {

    }
}
