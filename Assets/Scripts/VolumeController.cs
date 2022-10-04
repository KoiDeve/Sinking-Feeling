using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{

    static bool exists = false;

    float overallVolume = 0.8f;

    public AudioMixer currentVolume;

    private Slider slider;
    //private CanvasGroup alphaValue;

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
            //alphaValue = GetComponent<CanvasGroup>();
            exists = !exists;
            DontDestroyOnLoad(this);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        //alphaValue.alpha = level == 1 ? 1 : 0;
        if(level == 0) { 
            FindSlider();
            slider.value = overallVolume;
        }
        currentVolume.SetFloat("master", overallVolume);
    }

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

    public void SetNewValue() {
        if (slider == null) {
            FindSlider();
        }
        overallVolume = slider.value;
        currentVolume.SetFloat("master", overallVolume);
    }

}
