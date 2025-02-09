using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMainMenuScript : MonoBehaviour
{

    private GlowingEffectMainMenuScript mainMenuHelpsLastObjectSaved;

    void Update()
    {
        // Glowing Effect
        RaycastHit2D hitSomething = Physics2D.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction);
        if(hitSomething)
        {
            if(hitSomething.collider.gameObject != null && mainMenuHelpsLastObjectSaved != null)
            {
                if(hitSomething.collider.gameObject != mainMenuHelpsLastObjectSaved.gameObject)
                {
                    mainMenuHelpsLastObjectSaved.isGlowing = false;
                    Color defaultColor = new Color(1f, 1f, 1f, 1f); // Default
                    mainMenuHelpsLastObjectSaved.SwitcherGlowing(defaultColor);
                }
            }

            GlowingEffectMainMenuScript itemHitItemScript = hitSomething.collider.gameObject.GetComponent<GlowingEffectMainMenuScript>();
            if(itemHitItemScript == null) return; // SI C PO UN ITEM !!

            if(mainMenuHelpsLastObjectSaved != itemHitItemScript)
            {
                mainMenuHelpsLastObjectSaved = itemHitItemScript;
            }
            if(mainMenuHelpsLastObjectSaved != null)
            {
                mainMenuHelpsLastObjectSaved.isGlowing = true;

                Color glowingColorOfItem = new Color(1f, 1f, 1f, 1f); // Default
                itemHitItemScript.SwitcherGlowing(glowingColorOfItem);
            }
                
        }
        else
        {
            if(mainMenuHelpsLastObjectSaved != null)
            {
                mainMenuHelpsLastObjectSaved.isGlowing = false;
                Color defaultColor = new Color(1f, 1f, 1f, 1f);
                mainMenuHelpsLastObjectSaved.SwitcherGlowing(defaultColor);
            }
                    
        }
  
    }

}

