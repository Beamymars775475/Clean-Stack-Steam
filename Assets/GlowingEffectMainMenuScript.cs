using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingEffectMainMenuScript : MonoBehaviour
{

    public bool isGlowing;
    public void SwitcherGlowing(Color newShaderColor) // Need to be switched outside of the method
    {
        SpriteRenderer gbShader = gameObject.GetComponent<SpriteRenderer>();
        if(isGlowing)
        {
            gbShader.material.SetColor("_OutlineColor", newShaderColor);
            gbShader.material.EnableKeyword("OUTBASE_ON");           
        }
        else
        {
            gbShader.material.DisableKeyword("OUTBASE_ON");
        }
        
    }
}
