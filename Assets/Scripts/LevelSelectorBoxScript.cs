using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

public class LevelSelectorBoxScript : MonoBehaviour
{
    public Sprite[] normalStarSprites;
    public Sprite[] weirdStarSprites;
    void Start()
    {
        int sceneOfButton = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + gameObject.name + ".unity");

        int chanceToGetWeird = Random.Range(0, 2);
        GameObject star = gameObject.transform.GetChild(0).gameObject;
        Image starSpriteRenderer = star.GetComponent<Image>();

        Debug.Log("JE SUIS MOCHE");
        if(sceneOfButton < 0) return; // LE TEMPS DE TOUT SETUP ONLY
        
        if(GameManager.instance.levelsState[sceneOfButton-2] == 0)
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
        else if(GameManager.instance.levelsState[sceneOfButton-2] == 1)
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
        else if(GameManager.instance.levelsState[sceneOfButton-2] == 2)
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
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
