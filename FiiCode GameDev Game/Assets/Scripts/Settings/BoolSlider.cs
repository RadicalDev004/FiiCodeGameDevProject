using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BoolSlider : MonoBehaviour
{
    private Slider slider;
    public TMP_Text OnOff;

    public string ppName;
    public int InitialValue;
    void Start()
    {
        if (!PlayerPrefs.HasKey(ppName))
        {
            PlayerPrefs.SetInt(ppName, InitialValue);
        }

        slider = GetComponent<Slider>();

        slider.minValue = 0;
        slider.maxValue = 1;

        slider.value = PlayerPrefs.GetInt(ppName);
        UpdateOnOffSwitch();
    }

    public void OnSliderPress()
    {
        if (PlayerPrefs.GetInt(ppName) == 0)
        {
            PlayerPrefs.SetInt(ppName, 1);
            StartCoroutine(SliderAnimation(1));
        }
        else
        {
            PlayerPrefs.SetInt(ppName, 0);
            StartCoroutine(SliderAnimation(0));
        }
        UpdateOnOffSwitch();
    }

    IEnumerator SliderAnimation(int to)
    {
        bool grow = true;

        if (slider.value > to)
        {
            grow = false;
        }

        while (slider.value != to)
        {
            if (grow)
            {
                slider.value += 0.1f;
                yield return new WaitForSecondsRealtime(0.01f);
            }
            else
            {
                slider.value -= 0.1f;
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
    }

    private bool State()
    {
        if (PlayerPrefs.GetInt(ppName) == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void UpdateOnOffSwitch()
    {
        if (PlayerPrefs.GetInt(ppName) == 0)
        {
            OnOff.text = "Off";
        }
        else if (PlayerPrefs.GetInt(ppName) == 1)
        {
            OnOff.text = "On";
        }
    }
}
