using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool[] itemDiscovered;

    public bool[] levelsDone;

    public bool isHardMode;

    public int controlsChoice;

    public bool isInvTransparent;


    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        this.itemDiscovered = new bool[35];
        this.levelsDone = new bool[65];
        this.isHardMode = false;
        this.controlsChoice = 2;
        this.isInvTransparent = false;
    }
}
