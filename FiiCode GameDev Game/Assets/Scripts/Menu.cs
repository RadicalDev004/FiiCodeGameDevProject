using Pixelplacement;
using RadicalKit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public int LevelsCount;

    public Image Background;
    public Image Settings, Shop, Question;

    public TMP_Text StarsCount, EnergyCount, EnergyTimer;

    public int SCount;

    private bool AnimationCooldown;


    private void Awake()
    {
        Debug.LogWarning("Map scroll only supported on mobile!");

        Application.targetFrameRate = 60;
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
    }

    public void OpenSettings()
    {
        AudioManager.Play("OpenPanel");
        FillInBackground();
        OpenTab(Settings);
    }
    public void CloseSettings()
    {
        AudioManager.Play("ClosePanel");
        FillOutBackground();
        CloseTab(Settings);
    }


    public void OpenShop()
    {
        AudioManager.Play("OpenPanel");
        FillInBackground();
        OpenTab(Shop);
    }
    public void CloseShop()
    {
        AudioManager.Play("ClosePanel");
        FillOutBackground();
        CloseTab(Shop);
    }


    public void OpenQuestion()
    {
        AudioManager.Play("OpenPanel");
        FillInBackground();
        OpenTab(Question);
    }
    public void CloseQuestion()
    {
        AudioManager.Play("ClosePanel");
        FillOutBackground();
        CloseTab(Question);
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

        Minimize(Settings, Shop, Question);
    }

    private void GetStarsCount()
    {
        for (int i = 1; i <= LevelsCount; i++)
        {
            SCount += PlayerPrefs.GetInt("LevelStars" + i);
        }
    }

    private void Minimize(params Image[] imgs)
    {
        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i].GetComponent<RectTransform>().localScale = Vector3.zero;
        }
    }
}
