using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class itemScript : MonoBehaviour
{
    
    public string txtName;
    public string txtDescription;

    public MMF_Player explosionFeedbacks;
    public MMF_Player explosionFlickerFeedbacks;

    // Start is called before the first frame update
    void Start()
    {
        txtName = "Banana :";
        txtDescription = "<rot> Banana. </rot>";
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "ReadyToExplode" && GameManager.instance.activeStrangePotion)
        {
            explosionFeedbacks.PlayFeedbacks();
            explosionFlickerFeedbacks.StopFeedbacks();
            gameObject.tag = "Exploded";
        }
    }

    void OnCollisionEnter2D(Collision2D _col)
    {
        if(_col.gameObject.tag == "deadArea" && (gameObject.tag == "item" || gameObject.tag == "CantAcceptPotions"))
        {
            GameManager.instance.isGameOver = true;
        }

        if(_col.gameObject.tag == "nextArea" && (gameObject.tag == "item" || gameObject.tag == "CantAcceptPotions"))
        {
            GameManager.instance.goNextFloor = true;
        }
    }

}
