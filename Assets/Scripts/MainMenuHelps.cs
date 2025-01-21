using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuHelps : MonoBehaviour
{
    public SpriteRenderer spawnerSpriteRenderer;
    public GameObject Mask;
    void Start()
    {
        if(!GameManager.instance.firstTimeInMainMenuThisLaunch) 
        {
            gameObject.SetActive(false);
            Mask.SetActive(false);
        }

    }

}
