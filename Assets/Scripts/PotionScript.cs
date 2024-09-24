using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;


public class PotionScript : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform cursor;

    public MMF_Player feedbacks;
    
    
    public itemScript itemscript;

    void Start()
    {

    }


    void Update()
    {
        
    }

    public void CatchAObject()
    {
        GameObject prefab = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Transform prefabTransform = prefab.GetComponent<Transform>();
        prefabTransform.SetParent(cursor);
        feedbacks.PlayFeedbacks();
        GameManager.instance.isInventoryOpen = false;
        GameManager.instance.forceToCloseDescription = true;
        Destroy(gameObject);
    }


    public void SendingDescriptionInformations()
    {
        GameManager.instance.currentTextName = itemscript.txtName;
        GameManager.instance.currentTextDescription = itemscript.txtDescription;
    }
}
