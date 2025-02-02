using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject title;

    private Camera _mainCamera;
    void Start()
    {
        _mainCamera = Camera.main; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        GameManager.instance.goNextFloor = true;
    }


}
