using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int Level;
    private int Stars = 2;

    public bool HasCableStar = true;
    public bool HasChestStar = false;

    public GameObject[] UiStars;
    public TMP_Text Congrats;

    private void Awake()
    {
        AudioManager.Stop("Background");

        if (SceneManager.GetActiveScene().name == "LevelMaker")
            AudioManager.Play("MapMakerBackground");
        else
        {
            if (!AudioManager.IsPlaying("Level")) AudioManager.Play("Level");
        }



        try { Level = int.Parse(SceneManager.GetActiveScene().name.Replace("Level", "")); }
        catch (FormatException) { }


        HasCableStar = true;
        HasChestStar = false;
    }

    private void OnEnable()
    {
        Player.End += EndGameFromManager;
    }
    private void OnDisable()
    {
        Player.End -= EndGameFromManager;
    }

    private void EndGameFromManager()
    {
        FindObjectOfType<AnimationManager>().State = 25;
        Debug.Log("Ended game from Manager");
        if (!HasCableStar) Stars--;
        if (HasChestStar) Stars++;

        switch (Stars)
        {
            case 1:
                Congrats.text = "Awsome!";
                break;
            case 2:
                Congrats.text = "Very Good!";
                break;
            case 3:
                Congrats.text = "Perfect!";
                break;
        }

        if (Stars == 3 && SceneManager.GetActiveScene().name == "LevelMaker")
        {
            PlayerPrefs.SetInt("IsPlayTested", 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(Stars == 3 && SceneManager.GetActiveScene().name == "LevelOnline")
        {
            FindObjectOfType<LevelLoader>().Recievereward();
        }

        if (PlayerPrefs.GetInt(("LevelStars" + Level).ToString()) < Stars)
            PlayerPrefs.SetInt(("LevelStars" + Level).ToString(), Stars);

        Debug.Log(("LevelStars" + Level) + " " + Stars);
        if (PlayerPrefs.GetInt("Level") <= Level) PlayerPrefs.SetInt("Level", Level + 1);
        Debug.Log(PlayerPrefs.GetInt("Level"));
        StartCoroutine(StarAnimation(UiStars, 1, Stars));

        FindObjectOfType<Energy>().UseEnergy();

        //SceneManager.LoadScene("Level");
    }

    private IEnumerator StarAnimation(GameObject[] stars, float time, int count)
    {
        yield return new WaitForSecondsRealtime(0.6f);
        int i = 0;
        while (count > 0)
        {
            stars[i].transform.localScale = new Vector3(5, 5, 5);
            stars[i].SetActive(true);
            Pixelplacement.Tween.LocalScale(stars[i].GetComponent<RectTransform>(), Vector3.one, time, 0, Pixelplacement.Tween.EaseInStrong);
            i++;
            yield return new WaitForSeconds(time);
            AudioManager.Play("Star");
            yield return new WaitForSeconds(0.5f);
            count--;
        }
    }


}
