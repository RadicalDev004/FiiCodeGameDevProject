using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    public TMP_Text volume;

    private void Start()
    {
        if(!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 0.5f);
        }

        slider = GetComponent<Slider>();

        slider.minValue = 0;
        slider.maxValue = 1;

        slider.value = PlayerPrefs.GetFloat("Volume");
    }

    private void Update()
    {
        volume.text = ((int)(slider.value * 100)).ToString() +"%";
    }

    public void OnSliderTouch()
    {
        PlayerPrefs.SetFloat("Volume", slider.value);
        slider.value = PlayerPrefs.GetFloat("Volume");
        //Debug.Log(PlayerPrefs.GetFloat("Volume"));
    }
}
