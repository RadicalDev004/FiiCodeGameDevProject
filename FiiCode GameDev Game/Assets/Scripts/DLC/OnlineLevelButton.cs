using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class OnlineLevelButton : MonoBehaviour
{
    public string Name;
    public string Code;
    public string AstroPos;
    public string Reward;
    public string Difficulty;
    public int Rating;

    private TMP_Text TName, TReward, TDifficulty;
    private GameObject GRating;
    private Button LoadLevel;
    public Sprite E, M, D;



    private void Start()
    {
        TName = transform.Find("Name").GetComponent<TMP_Text>();
        TReward = transform.Find("Reward").GetComponent<TMP_Text>();
        TDifficulty = transform.Find("Difficulty").GetComponent<TMP_Text>();
        GRating = transform.Find("Rating").gameObject;
        LoadLevel = transform.Find("LoadLevel").GetComponent<Button>();

        TName.text = Name;
        TReward.text = "Reward: " + Reward;

        LoadLevel.onClick.AddListener(LoadLevelInScene);

        SetRating();
        SetDifficulty();
    }

    private void SetRating()
    {
        for(int i = 0; i < Rating; i++)
        {
            GRating.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void SetDifficulty()
    {
        int a = int.Parse(Difficulty);

        switch(a)
        {
            case 1:
                TDifficulty.text = "Difficulty: Easy";
                GetComponent<Image>().sprite = E;
                break;
            case 2:
                TDifficulty.text = "Difficulty: Medium";
                GetComponent<Image>().sprite = M;
                break;
            case 3:
                TDifficulty.text = "Difficulty: Hard";
                GetComponent<Image>().sprite = D;
                break;

        }
    }
    public void LoadLevelInScene()
    {
        PlayerPrefs.SetString("LoadedLevelCode", Code);
        PlayerPrefs.SetString("AstroPosLoadedLevel", AstroPos);
        PlayerPrefs.SetString("NameLoadedLevel", Name);
        PlayerPrefs.SetString("RewardLoadedLevel", Reward);

        SceneManager.LoadScene("LevelOnline");
    }
}
