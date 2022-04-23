using RadicalKit;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pixelplacement;
using TMPro;
using System;

public class UI : MonoBehaviour
{
    public Image Background, EndScreen, Menu;
    private GameManager Manager;
    public Image Load;

    private bool AnimationCooldown = false;

    private void Awake()
    {
        PrepareUI();
        Manager = FindObjectOfType<GameManager>();
        StartCoroutine(LoadSceneStart());
    }


    private void OnEnable()
    {
        Player.End += EndGameUI;
    }
    private void OnDisable()
    {
        Player.End -= EndGameUI;
    }



    public void ChangeSceneGeneric(string name)
    {
        StartCoroutine(LoadScene(name));
    }

    public void RestartGameButton()
    {
        AudioManager.Play("ButtonPress");
        if (Energy.currentEnergy == 0) { ChangeSceneGeneric("Menu"); return; }

        try { FindObjectOfType<Energy>().UseEnergy(); }
        catch (NullReferenceException) { Debug.LogWarning("Energy not reduced due to entering Level through Level Scene and not Menu Scene!"); }

        ChangeSceneGeneric(SceneManager.GetActiveScene().name);
    }

    public void OpenMenuButton()
    {
        OpenMenu();
    }
    public void CloseMenuButton()
    {
        CloseMenu();
    }

    public void NextLevel()
    {
        if (Energy.currentEnergy == 0) ChangeSceneGeneric("Menu");
        ChangeSceneGeneric("Level" + (Manager.Level+1).ToString());
    }



    public void EndGameUI()
    {
        Background.gameObject.SetActive(true);

        FillInBackground();
        OpenTab(EndScreen);
    }

    private void PrepareUI()
    {
        Background.color = Change.ColorA(Background.color, 0);
        Background.gameObject.SetActive(false);

        EndScreen.GetComponent<RectTransform>().localScale = Vector3.zero;
        Menu.GetComponent<RectTransform>().localScale = Vector3.zero;
    }


    private void OpenMenu()
    {
        AudioManager.Play("OpenPanel");
        Time.timeScale = 0;
        FillInBackground();
        OpenTab(Menu);
    }
    private void CloseMenu()
    {
        AudioManager.Play("ClosePanel");
        Time.timeScale = 1;
        FillOutBackground();
        CloseTab(Menu);
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
    private IEnumerator LoadScene(string name)
    {
        Load.gameObject.SetActive(true);
        Load.fillAmount = 0;
        while (Load.fillAmount < 1)
        {
            Load.fillAmount += 0.025f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(name);
    }
}
