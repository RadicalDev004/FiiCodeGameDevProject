using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.IO;
using Pixelplacement;
using RadicalKit;

public class LevelEditorUI : MonoBehaviour
{
    public Image Background, Editor;
    public Button PublishB;
    public GameObject DragHandler;
    private GameManager Manager;
    public Image Load;
    public Image[] Addables;
    public GameObject[] CloseOnPlaytest;
    public TMP_InputField MapName, Reward;
    public GameObject PushLevelSucces;
    public TMP_Text Cooldown;

    public Image[] dif;

    private bool AnimationCooldown = false;
    public void ChooseDifficulty(int diff)
    {
        for(int i = 0; i < dif.Length; i++)
        {
            if (i + 1 == diff)
                dif[i].color = Change.ColorA(dif[i].color, 255);
            else
                dif[i].color = Change.ColorA(dif[i].color, 0);
        }
        PlayerPrefs.SetInt("MapDifficulty", diff);
        Debug.Log(diff);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("IsPlayTested") == 1)
        {
            OpenEditorButton();
        }
    }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("MapDifficulty"))
        {
            PlayerPrefs.SetInt("MapDifficulty", 1);
        }


        MapName.text = PlayerPrefs.GetString("MapName");
        Reward.text = PlayerPrefs.GetString("MapReward");

        ChooseDifficulty(PlayerPrefs.GetInt("MapDifficulty"));
        PrepareUI();
        Manager = FindObjectOfType<GameManager>();
        StartCoroutine(LoadSceneStart());


        if(PlayerPrefs.GetInt("IsInPlayTestMode") == 1)
        {
            PlayTest();
        }
    }


    private void Update()
    {
        if (PlayerPrefs.GetInt("IsPlayTested") != 1)
            PublishB.interactable = false;

        if(PlayerPrefs.HasKey("PublishCooldown"))
        {            
            TimeSpan t = DateTime.Parse(PlayerPrefs.GetString("PublishCooldown")) - DateTime.Now;

            if(t.TotalSeconds > 0)
            {

                Cooldown.gameObject.SetActive(true);
                Cooldown.text = t.Hours + ":" + t.Minutes + ":" + t.Seconds;
                PublishB.interactable = false;
            }
            else
            {
                Cooldown.gameObject.SetActive(false);
                PublishB.interactable = true;
            }
        }
        else
        {
            Cooldown.gameObject.SetActive(false);
            PublishB.interactable = true;
        }
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

    public void OpenEditorButton()
    {
        OpenEditor();
    }
    public void CloseEditorButton()
    {
        CloseEditor();
    }


    public void PlayTest()
    {
        CloseEditor();
        DragHandler.SetActive(true);
        ManagePlayTest(false);
        TileEditorMaker.SelectedTile = -1;
        PlayerPrefs.SetInt("IsInPlayTestMode", 1);
    }

    public void StopPlayTest()
    {
        TileEditorMaker.SelectedTile = 0;
        PlayerPrefs.SetInt("IsInPlayTestMode", 0);
        Restart();
    }

    public void Restart()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
    }

    public void Publish()
    {
        if (PlayerPrefs.GetString("MapName") == "" || PlayerPrefs.GetString("MapReward") == "")
        {
            AudioManager.Play("Error");
            return;
        }
        DateTime d = DateTime.Now.AddHours(2);
        PlayerPrefs.SetString("PublishCooldown", d.ToString());
        StartCoroutine(PutMapInDatabase());

    }

    private IEnumerator PutMapInDatabase()
    {
        yield return new WaitUntil(() => DatabaseManager.IsReady);
        //Debug.LogError("Reached here");

        StartCoroutine(IPushMapDatabase("MapDifficulty", PlayerPrefs.GetInt("MapDifficulty").ToString()));
        StartCoroutine(IPushMapDatabase("MapReward", PlayerPrefs.GetString("MapReward")));
        StartCoroutine(IPushMapDatabase("MapCode", PlayerPrefs.GetString("MapMaker")));
        StartCoroutine(IPushMapDatabase("AstroPos", PlayerPrefs.GetInt("MapAstroStartPos").ToString()));
    }


    public void EndGameUI()
    {
        Background.gameObject.SetActive(true);

        FillInBackground();
        OpenTab(Editor);

        DragHandler.SetActive(false);
        ManagePlayTest(true);
        TileEditorMaker.SelectedTile = 0;
        PlayerPrefs.SetInt("IsInPlayTestMode", 0);

        Restart();
    }

    private void PrepareUI()
    {
        Background.color = Change.ColorA(Background.color, 0);
        Background.gameObject.SetActive(false);

        Editor.GetComponent<RectTransform>().localScale = Vector3.zero;

        
    }


    public void OpenAddables()
    {
        AudioManager.Play("MapMakerOpenAdd");
        for (int i = 0; i < Addables.Length; i++)
        {
            Tween.LocalPosition(Addables[i].GetComponent<RectTransform>(), new Vector3(0, -210 - i * 200, 0), 0.3f, 0, Tween.EaseOutBack);
        }
    }
    public void CloseAddables()
    {
        for (int i = 0; i < Addables.Length; i++)
        {
            Tween.LocalPosition(Addables[i].GetComponent<RectTransform>(), Vector3.zero, 0.3f, 0, Tween.EaseIn);
        }
    }


    private void OpenEditor()
    {
        AudioManager.Play("OpenPanel");
        //Time.timeScale = 0;
        FillInBackground();
        OpenTab(Editor);
    }
    private void CloseEditor()
    {
        AudioManager.Play("ClosePanel");
        //Time.timeScale = 1;
        FillOutBackground();
        CloseTab(Editor);
    }


    private void ManagePlayTest(bool activeStatus)
    {
        foreach(GameObject obj in CloseOnPlaytest)
        {
            obj.SetActive(activeStatus);
        }
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



    public void OnNameFieldValueChanged()
    {
        PlayerPrefs.SetString("MapName", MapName.text);
    }
    public void OnRewardFieldValueChanged()
    {
        PlayerPrefs.SetString("MapReward", Reward.text);
        Debug.Log(PlayerPrefs.GetString("MapReward"));
    }

    public void ClosePushLevelSucces()
    {
        Tween.LocalScale(PushLevelSucces.transform, Vector3.zero, 0.3f, 0, Tween.EaseIn);
    }


    public IEnumerator IPushMapDatabase(string path, string value)
    {
        var task = DatabaseManager.DbReference().Child("Maps").Child(PlayerPrefs.GetString("MapName")).Child(path).SetValueAsync(value);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(task.Exception);
        }
        else
        {
            Tween.LocalScale(PushLevelSucces.transform, Vector3.one, 0.3f, 0, Tween.EaseIn);
        }
    }
}
