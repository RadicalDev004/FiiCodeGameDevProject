using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Energy : MonoBehaviour
{
    public static string energyText;
    public static string timerText;

    private int maxEnergy = 5;
    public static int currentEnergy;
    private int restoreDuration = 5;

    private DateTime nextEnergyTime;
    private DateTime lastEnergyTime;

    private bool isRestoring = false;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("currentEnergy"))
        {
            PlayerPrefs.SetInt("currentEnergy", 5);
            Load();
            StartCoroutine(RestoreEnergy());
        }
        else
        {
            Load();
            StartCoroutine(RestoreEnergy());
        }
    }


    public void UseEnergy()
    {
        if (currentEnergy >= 1)
        {
            currentEnergy--;
            UpdateEnergy();
            Save();

            if (isRestoring == false)
            {
                if (currentEnergy + 1 == maxEnergy)
                {
                    nextEnergyTime = AddDuration(DateTime.Now, restoreDuration);
                }
                StartCoroutine(RestoreEnergy());
            }
        }
        else
        {

            Debug.Log("No more energy!!");
        }
    }


    private IEnumerator RestoreEnergy()
    {
        UpdateEnergyTimer();
        UpdateEnergy();
        isRestoring = true;

        while (currentEnergy < maxEnergy)
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime nextDateTime = nextEnergyTime;
            bool isEnergyAdding = false;

            while (currentDateTime > nextDateTime)
            {
                if (currentEnergy < maxEnergy)
                {
                    isEnergyAdding = true;
                    currentEnergy++;
                    UpdateEnergy();
                    DateTime timeToAdd = lastEnergyTime > nextDateTime ? lastEnergyTime : nextDateTime;
                    nextDateTime = AddDuration(timeToAdd, restoreDuration);
                }
                else
                {
                    break;
                }
            }

            if (isEnergyAdding == true)
            {
                lastEnergyTime = DateTime.Now;
                nextEnergyTime = nextDateTime;
            }
            UpdateEnergyTimer();
            UpdateEnergy();
            Save();
            yield return null;

        }
        isRestoring = false;
    }


    private DateTime AddDuration(DateTime datetime, int duration)
    {
        return datetime.AddMinutes(duration);
    }


    public void UpdateEnergyTimer()
    {
        if (currentEnergy >= maxEnergy)
        {
            timerText = "Full";
            return;
        }
        TimeSpan time = nextEnergyTime - DateTime.Now;
        string timeValue = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
        timerText = timeValue;
    }

    public void UpdateEnergy()
    {
        energyText = currentEnergy.ToString() + "/" + maxEnergy.ToString();
    }

    private DateTime StringToDate(string datetime)
    {
        if (string.IsNullOrEmpty(datetime))
        {
            return DateTime.Now;
        }
        else
        {
            return DateTime.Parse(datetime);
        }
    }

    public void Load()
    {
        currentEnergy = PlayerPrefs.GetInt("currentEnergy");
        nextEnergyTime = StringToDate(PlayerPrefs.GetString("nextEnergyTime"));
        lastEnergyTime = StringToDate(PlayerPrefs.GetString("lastEnergyTime"));
    }

    private void Save()
    {
        PlayerPrefs.SetInt("currentEnergy", currentEnergy);
        PlayerPrefs.SetString("nextEnergyTime", nextEnergyTime.ToString());
        PlayerPrefs.SetString("lastEnergyTime", lastEnergyTime.ToString());
    }

    public void GiveEnergy()
    {
        Load();
        UpdateEnergy();
        UpdateEnergyTimer();
    }
}



