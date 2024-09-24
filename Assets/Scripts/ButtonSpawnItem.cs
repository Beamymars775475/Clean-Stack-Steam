using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawnItem : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform cursor;

    public MMF_Player feedbacksInventory;


    public itemScript itemscript;

    public GameObject itemThrowed;

    public MMF_Player feedbacksBox;

    public Animator animator;

    public bool isLoadingAnimation;
    public bool canBeDestroy;

    public bool isDelivered;

    

    void Start()
    {

    }


    void Update()
    {
        if(itemPrefab.tag == "BiggerPotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#DD3737><wave>Red Potion</wave></color>";
            itemscript.txtDescription = "You can use this potion on a object to trigger <wave>weird</wave> things ! ";
        }

        else if(itemPrefab.tag == "ShrinkPotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#0097FF><wave>Blue Potion</wave></color>";
            itemscript.txtDescription = "You can use this potion on a object to trigger <wave>weird</wave> things ! ";
        }

        else if(itemPrefab.tag == "StrangePotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#1b9115><wave>Green Potion</wave></color>";
            itemscript.txtDescription = "You can use this potion on a object to trigger <wave>weird</wave> things ! ";
        }

        if(canBeDestroy && isLoadingAnimation == false)
        {
            Destroy(gameObject);
        }

    }

    public void CatchAObject()
    {
        if((itemPrefab.tag == "item" || itemPrefab.tag == "itemTable" ) && isDelivered == false)
        {
            Debug.Log("a");
            GameObject prefab = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Transform prefabTransform = prefab.GetComponent<Transform>();
            prefabTransform.SetParent(cursor);
            feedbacksInventory.PlayFeedbacks();
            GameManager.instance.isInventoryOpen = false;
            GameManager.instance.forceToCloseDescription = true;
            isDelivered = true;

            GameManager.instance.canAccessToInventory = false;

            

            feedbacksBox.PlayFeedbacks();
            
            StartCoroutine(WaitAnimation(0.80f));
            
        }

        else if((itemPrefab.tag == "BiggerPotion" || itemPrefab.tag == "ShrinkPotion" || itemPrefab.tag == "StrangePotion") && itemThrowed.transform.childCount > 0)
        {
            GameObject prefab = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Transform prefabTransform = prefab.GetComponent<Transform>();
            prefabTransform.SetParent(cursor);
            feedbacksInventory.PlayFeedbacks();
            GameManager.instance.isInventoryOpen = false;
            GameManager.instance.forceToCloseDescription = true;

            isDelivered = true;

            GameManager.instance.canAccessToInventory = false;

            Destroy(gameObject);


        }



    }


    public void SendingDescriptionInformations()
    {
        GameManager.instance.currentTextName = itemscript.txtName;
        GameManager.instance.currentTextDescription = itemscript.txtDescription;

        if(itemPrefab.tag != "itemTable")
        {
            GameManager.instance.needImageDark = false;
        }
        else if(itemPrefab.tag == "itemTable")
        {
            GameManager.instance.needImageDark = true;
        }
    }

    IEnumerator WaitAnimation(float cooldown)
    {
        if(isLoadingAnimation == false)
        {
            isLoadingAnimation = true;
            animator.SetTrigger("StartAnimBox");
            yield return new WaitForSeconds(cooldown);

            isLoadingAnimation = false;
            canBeDestroy = true;
        }

    }
}
