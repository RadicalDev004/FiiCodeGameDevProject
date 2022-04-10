using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    private TMP_Text Level;
    private GameObject Stars;
    private int level;

    public Sprite Completed, Locked, Current, Special;
    public bool isSpecial = false;

    private void Awake()
    {
        level = transform.GetSiblingIndex() + 1;

        Level = GetComponentInChildren<TMP_Text>();
        Level.text = level.ToString();

        Stars = transform.Find("Stars").gameObject;

        level = int.Parse(Level.text);

        GetComponent<Button>().onClick.AddListener(LoadLevel);

        SetStars();
        SetSprite();
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene("Level" + level);
    }

    private void SetStars()
    {
        int nr = PlayerPrefs.GetInt("LevelStars" + level);

        for(int i = 0; i < Stars.transform.childCount; i++)
        {
            if(i+1>nr)
            {
                Stars.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void SetSprite()
    {
        int currentLevel = PlayerPrefs.GetInt("Level");

        if(isSpecial)
            GetComponent<Image>().sprite = Special;
        else if(level == currentLevel)
            GetComponent<Image>().sprite = Current;
        else if(level<currentLevel)
            GetComponent<Image>().sprite = Completed;
        else if(level>currentLevel)
        {
            GetComponent<Image>().sprite = Locked;
            GetComponent<Button>().enabled = false;
        }
         
    }
}
