using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
public class AudioSettingsManager : MonoBehaviour
{
    public AudioMixer scrollBarsFeedbacks;

    public TextMeshProUGUI showVolumeMaster;

    public TextMeshProUGUI showVolumeMusic;

    public TextMeshProUGUI showVolumeSound;

    public int gra;
    

    // Start is called before the first frame update
    void Start()
    {   
        showVolumeMaster.text = "100%";
        showVolumeMusic.text = "70%";
        showVolumeSound.text = "70%";

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




}
