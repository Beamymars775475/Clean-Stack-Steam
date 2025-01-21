using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.InputSystem;

public class ClickingItemScript : MonoBehaviour
{
    private itemScript itemScript;

    private Camera _mainCamera;

    [Header("Clicking additional things")]

    public Sprite[] additionalSpriteSafe1;
    public Sprite additionalSprite1;

    void Start()
    {
        _mainCamera = Camera.main; // Setup Camera

        itemScript = gameObject.GetComponent<itemScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void OnClick(InputAction.CallbackContext context)
   {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider) return;
       
        if(rayHit.collider.gameObject == gameObject)
        {
            MMF_Player soundsWhenClickingFeedbacks = gameObject.transform.GetChild(11).GetComponent<MMF_Player>(); // 11eme enfant c le son/effet du click
            if(itemScript.cursor != null)
            {
                if(itemScript.cursor.transform.childCount == 0 && !soundsWhenClickingFeedbacks.IsPlaying && !itemScript.isTable)
                {
                    soundsWhenClickingFeedbacks.PlayFeedbacks();
                }
            }


            // CLICKABLE (Umbrella)
            if(itemScript.cursor != null)
            {
                if(itemScript.cursor.transform.childCount != 0)
                {
                    if(itemScript.isClickable && !itemScript.isReady2 && itemScript.cursor.transform.GetChild(0).tag != "ClonePotion")
                    {
                        itemScript.isClicked = true;
                        // Update Sprite

                        // DIFFERENT STATES TO CHANGE TO BIGGER BETTER
                        float delay = 0f;
                        foreach(Sprite sprite in additionalSpriteSafe1)
                        {
                            StartCoroutine(WaitForNonSafeSprite(0.009f+delay, sprite));
                            delay += 0.009f;
                        }
                        StartCoroutine(WaitForNonSafeSprite(0.009f+delay, additionalSprite1)); // VRAI SPRITE
                        gameObject.AddComponent<PolygonCollider2D>();
                        
                        
                    }
                }

                else
                {
                    itemScript.isClicked = true;
                    // Update Sprite
                    
                    // DIFFERENT STATES TO CHANGE TO BIGGER BETTER
                    float delay = 0f;
                    foreach(Sprite sprite in additionalSpriteSafe1)
                    {
                        StartCoroutine(WaitForNonSafeSprite(0.009f+delay, sprite));
                        delay += 0.009f;
                    }
                    StartCoroutine(WaitForNonSafeSprite(0.009f+delay, additionalSprite1)); // VRAI SPRITE
                    gameObject.AddComponent<PolygonCollider2D>();
                        
                        
                }
            }
            else if(itemScript.isClickable && !itemScript.isClicked)
            {
                itemScript.isClicked = true;
                // Update Sprite
                
                // DIFFERENT STATES TO CHANGE TO BIGGER BETTER
                float delay = 0f;
                foreach(Sprite sprite in additionalSpriteSafe1)
                {
                    StartCoroutine(WaitForNonSafeSprite(0.009f+delay, sprite));
                    delay += 0.009f;
                }
                StartCoroutine(WaitForNonSafeSprite(0.009f+delay, additionalSprite1)); // VRAI SPRITE
                gameObject.AddComponent<PolygonCollider2D>();
                    
                    
            }


        }

   }

    private IEnumerator WaitForNonSafeSprite(float waitTime, Sprite newSprite)
    {
        yield return new WaitForSeconds(waitTime);

        // Update Sprite
        SpriteRenderer gbSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gbSpriteRenderer.sprite = newSprite;
        PolygonCollider2D gbPolygonCollider2D = gameObject.GetComponent<PolygonCollider2D>();
        Destroy(gbPolygonCollider2D);

    
        gameObject.AddComponent<PolygonCollider2D>();

        if(newSprite == additionalSprite1)
        {
            StartCoroutine(CooldownForSecReady2(1f));
        }

    }

    private IEnumerator CooldownForSecReady2(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        itemScript.isReady2 = true;
    }
}
