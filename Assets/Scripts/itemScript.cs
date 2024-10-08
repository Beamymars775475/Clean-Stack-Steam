using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class itemScript : MonoBehaviour
{
    
    [Header("ID")]
    public int itemID;

    [Header("Other")]

    public string txtName;
    public string txtDescription;

    public MMF_Player explosionFeedbacks;
    public MMF_Player explosionFlickerFeedbacks;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AlreadyUsedItem[itemID] = true;

        txtName = "Banana :";
        txtDescription = "<rot> Banana. </rot>";
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "ReadyToExplode" && GameManager.instance.activeStrangePotion)
        {
            Rigidbody2D gameObjectRb = gameObject.GetComponent<Rigidbody2D>();

            gameObjectRb.mass = gameObjectRb.mass *= 3f; // Par 3 car de base il est a /2 et comme c'est la potion étrange il passe de super petit à super gros
            explosionFeedbacks.PlayFeedbacks();
            explosionFlickerFeedbacks.StopFeedbacks();
            gameObject.tag = "Exploded";
        }
        if(explosionFlickerFeedbacks != null) // Si il existe
        {
            if(explosionFlickerFeedbacks.IsPlaying && GameManager.instance.activeStrangePotion)
            {
                gameObject.tag = "ReadyToExplode";
            }
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
