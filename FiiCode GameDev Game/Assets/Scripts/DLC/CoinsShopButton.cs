using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RadicalKit;

public class CoinsShopButton : MonoBehaviour
{
    public int Cost;
    public int Reward;
    public Image Lock;
    private Button Buy;

    private void Awake()
    {
        Buy = GetComponent<Button>();
        Buy.onClick.AddListener(BuyEnergy);      
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("OnlineCurrency") < Cost)
        {
            Lock.gameObject.SetActive(true);
        }
        else
        {
            Lock.gameObject.SetActive(false);
        }
    }

    public void BuyEnergy()
    {
        if (PlayerPrefs.GetInt("OnlineCurrency") < Cost)
            return;

        Prefs.DecreaseInt("OnlineCurrency", Cost);

        FindObjectOfType<Energy>().GiveEnergy();
    }
}
