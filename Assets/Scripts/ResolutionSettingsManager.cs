using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ResolutionSettingsManager : MonoBehaviour, IDataPersistence
{

    Resolution[] resolutions;
    List<int> resolutionsGoodRatioIndexList = new List<int>();

    public TMP_Dropdown resolutionDropdown;

    [Header("Resolution Screen Preferences")]

    public Toggle toggleFullScreen;

    public int indexReso;


    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        indexReso = resolutionIndex;
        Resolution resolution = resolutions[resolutionsGoodRatioIndexList[resolutionIndex]]; // resolutionsGoodRatioIndexList donne le vrai Index
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void LoadData(GameData data)
    {


        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, data.isFullScreen);
        Debug.Log(data.isFullScreen);
        toggleFullScreen.isOn = data.isFullScreen;


        indexReso = data.resolutionToUse; // Par défaut

        
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }). Distinct().ToArray();
        resolutionDropdown.ClearOptions();
            
        List<string> options = new List<string>();
        resolutionsGoodRatioIndexList = new List<int>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            if((float)resolutions[i].width/resolutions[i].height > 1.7f && (float)resolutions[i].width/resolutions[i].height < 1.8f)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);
                resolutionsGoodRatioIndexList.Add(i);
            }

        } 

        if(indexReso == -1)
        {
            indexReso = resolutionsGoodRatioIndexList.Count-1;
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
        data.resolutionToUse = indexReso;
        resolutionDropdown.value = indexReso;
    } 

}
