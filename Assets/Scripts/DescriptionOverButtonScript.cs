using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DescriptionOverButtonScript : MonoBehaviour
{
    public RectTransform m_parent;
    public Camera m_uiCamera;
    public RectTransform m_image;
    public Canvas m_canvas;

    public bool canShowDescription;

    public bool notOnItem;

    public GameObject textName;
    public GameObject textDescription;

    public TextMeshProUGUI textNameContent;
    public TextMeshProUGUI textDescriptionContent;


    public Sprite imageDark;
    public Sprite imageDefault;

    public MMF_Player feedbacksBottle;

    public Coroutine animationState;




    void Start()
    {
        gameObject.GetComponent<Image>().enabled = false;
        textName.SetActive(false);
        textDescription.SetActive(false);
    }

    void Update()
    {
        if(GameManager.instance.forceToCloseDescription)
        {
            gameObject.GetComponent<Image>().enabled = false;
            notOnItem = false;
            textName.SetActive(false);
            textDescription.SetActive(false);
            GameManager.instance.forceToCloseDescription = false;
        }

        Image imageGameObject = GetComponentInChildren<Image>();

        if(GameManager.instance.needImageDark)
        {
            imageGameObject.sprite = imageDark;
        }
        else
        {
            imageGameObject.sprite = imageDefault;
        }
        
    }

    public void WhenExitingUI(BaseEventData eventData)
    { 
        if(gameObject != null)
        {
            StopCoroutine(animationState);
            gameObject.GetComponent<Image>().enabled = false;
            notOnItem = false;
            textName.SetActive(false);
            textDescription.SetActive(false);
        }
    }

    public void WhenOnUI(BaseEventData eventData, RectTransform gameObjectPointed)
    {
        animationState = StartCoroutine(CooldownForDescription(0.65f, gameObjectPointed));
        notOnItem = true;

    }

    IEnumerator CooldownForDescription(float cooldown, RectTransform gameObjectToGoTo)
    {
        canShowDescription = false;


        yield return new WaitForSeconds(cooldown);
        if(notOnItem == true && GameManager.instance.isInventoryOpen)
        {
            ButtonSpawnItem buttonSpawnItemBox = gameObjectToGoTo.GetComponentInChildren<ButtonSpawnItem>(); // Pour avoir l'index


            textNameContent.text = GameManager.instance.currentTextName;

            if(GameManager.instance.modeHard) // Mode Hard
            {
                textNameContent.text = "<shake>????</shake>";
                textDescriptionContent.text = "<incr>????????????????????????????????????????????????????????????????????????????????????</incr>";
            }
            else if(GameManager.instance.AlreadyUsedItem[buttonSpawnItemBox.itemscript.itemID] == true) // Si on connaît l'item
            {
                textDescriptionContent.text = GameManager.instance.currentTextDescription;
            }
            else // Connaît pas encore l'item mais pas non plsu en hardcore
            {
                textDescriptionContent.text = "<incr>??????????????</incr>";                
            }
                




            if(gameObjectToGoTo.childCount != 0) // Pour les items
            {
                m_image.anchoredPosition = new Vector2(gameObjectToGoTo.anchoredPosition.x+300f, gameObjectToGoTo.anchoredPosition.y+115f);
            }
            else // Pour les potions 
            {
                m_image.anchoredPosition = new Vector2(gameObjectToGoTo.anchoredPosition.x+250f, gameObjectToGoTo.anchoredPosition.y+100f);
            }

            gameObject.GetComponent<Image>().enabled = true;
            textName.SetActive(true);
            textDescription.SetActive(true);
        }

        canShowDescription = true;
    }


}
