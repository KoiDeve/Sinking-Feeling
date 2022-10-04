using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

// Controls the volume for the game. Is carried throughout the game.
public class VolumeController : MonoBehaviour
{

    // Volume levels, canvas elements, mixers, and current states of the object that holds this script.
    static bool exists = false;
    float overallVolume = 0.8f;
    public AudioMixer currentVolume;
    private Slider slider;

    // Checks to make sure only one object exists in the game. Finds all necessary components.
    private void Start()
    {
        if (exists)
        {
            Destroy(this);
        }
        else {
            FindSlider();
            slider.value = overallVolume;
            currentVolume.SetFloat("master", -30);
            slider.value = -30;
            exists = !exists;
            DontDestroyOnLoad(this);
        }
    }

    // When the title screen is loaded, loads the presets the user implemented (default values upon startup).
    private void OnLevelWasLoaded(int level)
    {
        if(level == 0) { 
            FindSlider();
            slider.value = overallVolume;
        }
        currentVolume.SetFloat("master", overallVolume);
    }

    // Finds the slider on the title screen and gives it the permission to control the volume.
    private void FindSlider() {
        try
        {
            slider = GameObject.Find("Volume Slider").GetComponent<Slider>();
            slider.onValueChanged.AddListener(delegate { SetNewValue(); });
        }
        catch (System.Exception e) {
            Debug.Log("Volume Slider not found!");
        }
    }

    // Sets the new value of the slider for the volume levels.
    public void SetNewValue() {
        if (slider == null) {
            FindSlider();
        }
        overallVolume = slider.value;
        currentVolume.SetFloat("master", overallVolume);
    }

}
