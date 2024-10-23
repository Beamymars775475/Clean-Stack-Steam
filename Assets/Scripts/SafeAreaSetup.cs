using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SafeAreaSetup : MonoBehaviour
{
    void Start()
    {
        Mask gameobjectMask = gameObject.AddComponent<Mask>();
        gameobjectMask.showMaskGraphic = false;

    }
}
