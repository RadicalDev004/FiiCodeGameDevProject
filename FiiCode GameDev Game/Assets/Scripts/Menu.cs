using Pixelplacement;
using RadicalKit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
    public int LevelsCount;

    public Image Background;
    public Image Settings, Shop, Question, NewContent, CoinsShop;
    public Image Load;

    public TMP_Text StarsCount, EnergyCount, EnergyTimer, CoinsCount;

    public int SCount;

    private bool AnimationCooldown;


    private void Awake()
    {
        Debug.LogWarning("Map scroll only supported on mobile!");
        StartCoroutine(LoadSceneStart());

        if(!PlayerPrefs.HasKey("OnlineCurrency"))
        {
            PlayerPrefs.SetInt("OnlineCurrency", 0);
        }
     
        Time.timeScale = 1;

        PrepareUI();
        GetStarsCount();

        StarsCount.text = SCount.ToString();
    }

    private void Start()
    {
        AudioManager.StopAll();
        AudioManager.Play("Background");

    }


    private void Update()
    {
        EnergyCount.text = Energy.energyText;
        EnergyTimer.text = Energy.timerText;


        CoinsCount.text = PlayerPrefs.GetInt("OnlineCurrency").ToString("#,#").Replace(',', ' ');
    }

    public void OpenGenericPanel(Image img)
    {
        AudioManager.Play("OpenPanel");
        FillInBackground();
        OpenTab(img);
    }
    public void CloseGenericPanel(Image img)
    {
        AudioManager.Play("ClosePanel");
        FillOutBackground();
        CloseTab(img);
    }


    private void FillInBackground()
    {
        Background.gameObject.SetActive(true);
        Tween.Color(Background, new Color(0, 0, 0, 0.4f), 0.5f, 0, Tween.EaseInOut);
    }
    private void FillOutBackground()
    {
        Tween.Color(Background, new Color(0, 0, 0, 0), 0.5f, 0, Tween.EaseInOut);
        Invoke(nameof(DeActivateBackground), 0.5f);
    }
    private void DeActivateBackground() => Background.gameObject.SetActive(false);


    private void OpenTab(Image image)
    {
        if (AnimationCooldown) return;

        AnimationCooldown = true;
        Tween.LocalScale(image.GetComponent<RectTransform>(), Vector3.one, 0.5f, 0, Tween.EaseInOut);
    }
    private void CloseTab(Image image)
    {
        Tween.LocalScale(image.GetComponent<RectTransform>(), Vector3.zero, 0.5f, 0, Tween.EaseInOut);
        Invoke(nameof(ResetAnimationCooldown), 0.5f);
    }
    private void ResetAnimationCooldown() => AnimationCooldown = false;


    private void PrepareUI()
    {
        Background.color = Change.ColorA(Background.color, 0);
        Background.gameObject.SetActive(false);

        Minimize(Settings, Shop, Question, NewContent, CoinsShop);
    }

    private void GetStarsCount()
    {
        for (int i = 1; i <= LevelsCount; i++)
        {
            SCount += PlayerPrefs.GetInt("LevelStars" + i);
            //Debug.Log(i + " " + PlayerPrefs.GetInt("LevelStars" + i) + " " + SCount);
        }
    }

    private void Minimize(params Image[] imgs)
    {
        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i].GetComponent<RectTransform>().localScale = Vector3.zero;
        }
    }

    private IEnumerator LoadSceneStart()
    {
        Load.gameObject.SetActive(true);

        Load.fillAmount = 1;
        while (Load.fillAmount > 0)
        {
            Load.fillAmount -= 0.025f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        Load.gameObject.SetActive(false);
    }
}
