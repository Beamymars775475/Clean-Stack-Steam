using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ResolutionSettingsManager : MonoBehaviour, IDataPersistence
{

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;

    [Header("Resolution Screen Preferences")]

    public Toggle toggleFullScreen;

    public int indexReso;

    void Awake()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }). Distinct().ToArray();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        indexReso = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void LoadData(GameData data)
    {
        Screen.fullScreen = data.isFullScreen;
        toggleFullScreen.isOn = data.isFullScreen;


        indexReso = data.resolutionToUse; // Par défaut

        
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }). Distinct().ToArray();
        resolutionDropdown.ClearOptions();
            
        List<string> options = new List<string>();
            

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);


            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && data.firstTimeSetupResolution == true)
            {
                data.firstTimeSetupResolution = false;
                indexReso = i;
                Debug.Log("ZAWOUMBA");
            }
        } 

        
        SetResolution(indexReso);
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = indexReso;
        resolutionDropdown.RefreshShownValue();

    }

    public void SaveData(GameData data)
    {
        data.isFullScreen = Screen.fullScreen;
        toggleFullScreen.isOn = Screen.fullScreen;
        Debug.Log(indexReso);
        data.resolutionToUse = indexReso;
        resolutionDropdown.value = indexReso;
        resolutionDropdown.RefreshShownValue();
    } 

}
