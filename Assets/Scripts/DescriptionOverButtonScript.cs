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

    public void WhenExitingUI()
    {
        gameObject.GetComponent<Image>().enabled = false;
        notOnItem = false;
        textName.SetActive(false);
        textDescription.SetActive(false);
    }

    public void WhenOnUI()
    {
        StartCoroutine(CooldownForDescription(0.65f));
        notOnItem = true;

    }

    IEnumerator CooldownForDescription(float cooldown)
    {
        canShowDescription = false;
        yield return new WaitForSeconds(cooldown);
        if(notOnItem == true)
        {
            Vector2 anchoredPos;
            textNameContent.text = GameManager.instance.currentTextName;
            textDescriptionContent.text = GameManager.instance.currentTextDescription;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parent, Input.mousePosition, m_canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_uiCamera, out anchoredPos);
            m_image.anchoredPosition = anchoredPos;
            gameObject.GetComponent<Image>().enabled = true;
            textName.SetActive(true);
            textDescription.SetActive(true);
        }



        canShowDescription = true;
    }


}
