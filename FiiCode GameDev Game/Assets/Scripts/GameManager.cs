using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class GameManager : MonoBehaviour
{
    private int Level;
    private int Stars = 2;

    public bool HasCableStar = true;
    public bool HasChestStar = false;

    public GameObject[] UiStars;

    private void Awake()
    {
        AudioManager.StopAll();
        AudioManager.Play("Level");

        Level = int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));

        Application.targetFrameRate = 60;

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



        try { if (Stars < 3) FindObjectOfType<Energy>().UseEnergy(); }
        catch(NullReferenceException) { Debug.LogWarning("Energy not reduced due to entering Level through Level Scene and not Menu Scene!"); }

        if (PlayerPrefs.GetInt(("LevelStars" + Level).ToString()) < Stars)
            PlayerPrefs.SetInt(("LevelStars" + Level).ToString(), Stars);

        Debug.Log(("LevelStars" + Level) + " " + Stars);
        PlayerPrefs.SetInt("Level", Level + 1);

        StartCoroutine(StarAnimation(UiStars, 1, Stars));

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
