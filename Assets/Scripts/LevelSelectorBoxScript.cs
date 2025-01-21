using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.UI;

public class LevelSelectorBoxScript : MonoBehaviour
{
    public int indexSceneOfButton;

    public int worldIndex;
    public Sprite[] normalStarSprites;
    public Sprite[] weirdStarSprites;
    void Start()
    {
        int chanceToGetWeird = Random.Range(0, 2);
        GameObject star = gameObject.transform.GetChild(0).gameObject;
        Image starSpriteRenderer = star.GetComponent<Image>();


        if(GameManager.instance.levelsState[GameManager.instance.MONDES[worldIndex] + indexSceneOfButton] == 0)
        {
            if(chanceToGetWeird == 0)
            {
                starSpriteRenderer.sprite = normalStarSprites[0];
            }
            else
            {
                starSpriteRenderer.sprite = weirdStarSprites[0];
            }
        }
        else if(GameManager.instance.levelsState[GameManager.instance.MONDES[worldIndex] + indexSceneOfButton] == 1)
        {
            if(chanceToGetWeird == 0)
            {
                starSpriteRenderer.sprite = normalStarSprites[1];
            }
            else
            {
                starSpriteRenderer.sprite = weirdStarSprites[1];
            }
        }
        else if(GameManager.instance.levelsState[GameManager.instance.MONDES[worldIndex] + indexSceneOfButton] == 2)
        {
            if(chanceToGetWeird == 0)
            {
                starSpriteRenderer.sprite = normalStarSprites[2];
            }
            else
            {
                starSpriteRenderer.sprite = weirdStarSprites[2];
            }
        }
        
        if(indexSceneOfButton == 0 && GameManager.instance.levelsState[GameManager.instance.MONDES[worldIndex] + indexSceneOfButton + 1] == 0 && GameManager.instance.levelsState[GameManager.instance.MONDES[worldIndex] + indexSceneOfButton] == 0) // + 1 POUR LE PROCHAIN
        {
            Image gbShader = gameObject.GetComponent<Image>();

            Color newShaderColor = new Color(1f, 1f, 1f, 0f);
            gbShader.material.SetColor("_OutlineColor", newShaderColor);
            gbShader.material.EnableKeyword("OUTBASE_ON");     
            // Not interactable
            Button gbButton = gameObject.GetComponent<Button>();
            gbButton.transition = Selectable.Transition.ColorTint;
        }
        else if(indexSceneOfButton != 0 && GameManager.instance.levelsState[GameManager.instance.MONDES[worldIndex] + indexSceneOfButton - 1] == 1 && GameManager.instance.levelsState[GameManager.instance.MONDES[worldIndex] + indexSceneOfButton + 1] == 0 && GameManager.instance.levelsState[GameManager.instance.MONDES[worldIndex] + indexSceneOfButton + 1] == 0 && GameManager.instance.levelsState[GameManager.instance.MONDES[worldIndex] + indexSceneOfButton] == 0)
        {
            Image gbShader = gameObject.GetComponent<Image>();

            Color newShaderColor = new Color(1f, 1f, 1f, 0f);
            gbShader.material.SetColor("_OutlineColor", newShaderColor);
            gbShader.material.EnableKeyword("OUTBASE_ON");    
            // Not interactable
            Button gbButton = gameObject.GetComponent<Button>();
            gbButton.transition = Selectable.Transition.ColorTint;
        }
        else if(indexSceneOfButton != 0 && GameManager.instance.levelsState[GameManager.instance.MONDES[worldIndex] + indexSceneOfButton] == 0) // SI IL EST PAS FINI ET QUE C PAS LE PREMIER
        {
            // Shadowed
            Image imageBox = gameObject.GetComponent<Image>();
            imageBox.color -= new Color(0.75f, 0.75f, 0.75f, 0f);
            // Not interactable
            Button gbButton = gameObject.GetComponent<Button>();
            gbButton.transition = Selectable.Transition.None;
        }

    }

}
