using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class GameData
{
    public bool[] itemDiscovered;

    public bool[] levelsDone;

    [Header("Controls and more")]
    public bool isHardMode;

    public int controlsChoice;

    public bool isInvTransparent;

    [Header("Audio")]

    public float volumeMain;

    public float volumeMusic;

    public float volumeSound;

    [Header("Resolution")]

    public bool isFullScreen;

    public int resolutionToUse;

    [Header("First Time Played Checker")]

    public bool isItFirstTime;


    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load

    public GameData() 
    {
        this.itemDiscovered = new bool[35];
        this.levelsDone = new bool[65];

        this.isHardMode = false;
        this.controlsChoice = 2;
        this.isInvTransparent = false;

        this.volumeMain = 0;
        this.volumeMusic = 0;
        this.volumeSound = 0;

        this.resolutionToUse = -1;
        this.isFullScreen = true;

        this.isItFirstTime = true;
    }
}