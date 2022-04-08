using RadicalKit;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pixelplacement;

public class UI : MonoBehaviour
{
    public Image Background, EndScreen, Menu;
    private GameManager Manager;
    private bool AnimationCooldown = false;

    private void Awake()
    {
        PrepareUI();
        Manager = FindObjectOfType<GameManager>();
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
        SceneManager.LoadScene(name);
    }

    public void RestartGameButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenMenuButton()
    {
        OpenMenu();
    }
    public void ColseMenuButton()
    {
        CloseMenu();
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
        Time.timeScale = 0;
        FillInBackground();
        OpenTab(Menu);
    }
    private void CloseMenu()
    {
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
}
