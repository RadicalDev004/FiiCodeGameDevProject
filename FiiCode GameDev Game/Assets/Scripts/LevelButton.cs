using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    private TMP_Text Level;
    private GameObject Stars;
    private int level;
    public GameObject Line;
    public Image Load;

    public Sprite Completed, Locked, Current, Special;
    public bool isSpecial = false;

    private void Awake()
    {
        level = transform.GetSiblingIndex();

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
        if (Energy.currentEnergy == 0) { AudioManager.Play("Error"); return; }
        AudioManager.Play("EnterGame");

        StartCoroutine(LoadScene("Level" + level));
    }

    private void SetStars()
    {
        int nr = PlayerPrefs.GetInt("LevelStars" + level);

        for (int i = 0; i < Stars.transform.childCount; i++)
        {
            if (i + 1 > nr)
            {
                Stars.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void SetSprite()
    {
        int currentLevel = PlayerPrefs.GetInt("Level");

        if (isSpecial && level > currentLevel)
        {
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().sprite = Special;
        }
        else if (isSpecial && level <= currentLevel)
        {
            GetComponent<Image>().sprite = Special;
        }
        else if (level == currentLevel)
            GetComponent<Image>().sprite = Current;
        else if (level < currentLevel)
            GetComponent<Image>().sprite = Completed;
        else if (level > currentLevel)
        {
            GetComponent<Image>().sprite = Locked;
            GetComponent<Button>().enabled = false;
        }

    }
    private void SpawnLine()
    {
        Vector2 firstPos = GetComponent<RectTransform>().anchoredPosition;
        Vector2 lastPos = transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<RectTransform>().anchoredPosition;

        Vector2 desiredPos = (firstPos + lastPos) / 2;
        GameObject newLine = Instantiate(Line, transform);
        newLine.GetComponent<RectTransform>().anchoredPosition = desiredPos;
        LeanTween.rotate(newLine, new Vector3(0, 0, Vector2.Angle(firstPos, lastPos)), 0.1f);
    }

    private IEnumerator LoadScene(string name)
    {
        Load.gameObject.SetActive(true);
        while (Load.fillAmount < 1)
        {
            Load.fillAmount += 0.025f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(name);
    }
}
