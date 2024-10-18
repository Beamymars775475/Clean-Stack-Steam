using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAreaMainMenu : MonoBehaviour
{

    public MainMenuLever mainMenuLever;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D _col)
    {

        if((_col.gameObject.tag == "ButtonQuit" || _col.gameObject.tag == "ButtonFreeSpace" || _col.gameObject.tag == "ButtonSettings") && !mainMenuLever.leverBool) 
        {
            _col.gameObject.transform.localPosition = new Vector3(5.5f, 8f, 0); // Co de au-dessus la boîte
        }
        else
        {
            Destroy(_col.gameObject);
        }

        
    }
}
