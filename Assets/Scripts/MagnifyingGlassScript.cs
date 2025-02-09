using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MoreMountains;
using MoreMountains.Feedbacks;
using UnityEngine.UI;

public class MagnifyingGlassScript : MonoBehaviour
{
    public MMF_Player feedbacksInvisibleOn;

    private MMF_ImageAlpha feedbacksAlphaOn;
    public MMF_Player feedbacksInvisibleOff;

    private MMF_ImageAlpha feedbacksAlphaOff;

    private GameObject inventory;

    void Start()
    {
        inventory = gameObject.transform.parent.gameObject;
        Debug.Log(inventory);
    }
    public void OnMagnifyingGlass()
    {
        if(feedbacksInvisibleOff.IsPlaying)
        {
            feedbacksInvisibleOff.StopFeedbacks();


        }


            // SETUP ZERO VALUE
        Graphic inventoryImage = inventory.GetComponent<Graphic>(); // ALPHA A METTRE EN CURVEREMAPZERO

        feedbacksAlphaOn = feedbacksInvisibleOn.GetFeedbackOfType<MMF_ImageAlpha>();
        feedbacksAlphaOn.CurveRemapZero = inventoryImage.color.a;

        feedbacksAlphaOn.Duration = 0.5f*inventoryImage.color.a;

        feedbacksInvisibleOn.PlayFeedbacks();
    }

    public void OffMagnifyingGlass()
    {
        if(feedbacksInvisibleOn.IsPlaying)
        {
            feedbacksInvisibleOn.StopFeedbacks();

        }
        // SETUP ONE VALUE
        Graphic inventoryImage = inventory.GetComponent<Graphic>(); // ALPHA A METTRE EN CURVEREMAPONE

        feedbacksAlphaOff = feedbacksInvisibleOff.GetFeedbackOfType<MMF_ImageAlpha>();
        feedbacksAlphaOff.CurveRemapZero = inventoryImage.color.a;

        feedbacksAlphaOff.Duration = 0.5f*(1-inventoryImage.color.a);


        feedbacksInvisibleOff.PlayFeedbacks();
    }

    public void OnStartFeedbacksOn() // update items alpha
    {
        foreach(Transform item in inventory.transform)
        {
            if(item != gameObject && (item.tag == "Box" || item.tag == "BoxPotion"))
            {
                item.gameObject.SetActive(false);
            }
            
        }
    }

    public void OnStopFeedbacksOff() // update items alpha
    {
        foreach(Transform item in inventory.transform)
        {
            if(item != gameObject && (item.tag == "Box" || item.tag == "BoxPotion"))
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
