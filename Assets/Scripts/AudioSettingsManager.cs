using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioSettingsManager : MonoBehaviour, IDataPersistence
{
    public AudioMixer scrollBarsFeedbacks;

    public TextMeshProUGUI showVolumeMaster;

    public TextMeshProUGUI showVolumeMusic;

    public TextMeshProUGUI showVolumeSound;

    public Scrollbar sliderMain;

    public Scrollbar sliderMusic;

    public Scrollbar sliderSound;

    public int gra;

    // Start is called before the first frame update
    void Start()
    {   
        showVolumeMaster.text = "100%";
        showVolumeMusic.text = "63%";
        showVolumeSound.text = "63%";
    }

    public void SetMasterVolume(float volume)
    {
        if(volume != 0)
        {
            scrollBarsFeedbacks.SetFloat("MasterVolume", (55*volume)-55); 
        }
        else
        {
            scrollBarsFeedbacks.SetFloat("MasterVolume", -80); 
        }

        showVolumeMaster.text = ((int) (volume*100)).ToString()  + "%";
    }

    public void SetMusicVolume(float volume)
    {
        if(volume != 0)
        {
        scrollBarsFeedbacks.SetFloat("MusicVolume", (55*volume)-35);   
        }
        else
        {
            scrollBarsFeedbacks.SetFloat("MusicVolume", -80);           
        }

        showVolumeMusic.text = ((int) (volume*100)).ToString()  + "%";
    }

    public void SetSoundVolume(float volume)
    {
        if(volume != 0)
        {
            scrollBarsFeedbacks.SetFloat("SfxVolume", (55*volume)-35);   
        }
        else
        {
            scrollBarsFeedbacks.SetFloat("SfxVolume", -80);           
        }

        showVolumeSound.text = ((int) (volume*100)).ToString() + "%";
    }


    public void LoadData(GameData data)
    {
        scrollBarsFeedbacks.SetFloat("MasterVolume", data.volumeMain); 
        sliderMain.value = (data.volumeMain+55)/55;
        scrollBarsFeedbacks.SetFloat("MusicVolume", data.volumeMusic); 
        sliderMusic.value = (data.volumeMusic+35)/55;
        scrollBarsFeedbacks.SetFloat("SfxVolume", data.volumeSound); 
        sliderSound.value = (data.volumeSound+35)/55;
    }

    public void SaveData(GameData data)
    {
        scrollBarsFeedbacks.GetFloat("MasterVolume", out data.volumeMain); 
        scrollBarsFeedbacks.GetFloat("MusicVolume", out data.volumeMusic); 
        scrollBarsFeedbacks.GetFloat("SfxVolume", out data.volumeSound); 
    } 

}
