using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Credits : MonoBehaviour
{
    public GameObject Cr;

    private void Awake()
    {
        Cr.SetActive(false);
    }

    public void ShowCredits(float time)
    {
        AudioManager.StopAll();
        Cr.SetActive(true);
        LeanTween.move(Cr.GetComponent<RectTransform>(), new Vector3(0, -Cr.GetComponent<RectTransform>().anchoredPosition.y, 0), time);
        Invoke(nameof(ResetCredits), time+1);
    }
    public void ResetCredits()
    {
        Cr.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -Cr.GetComponent<RectTransform>().anchoredPosition.y, 0);
        LeanTween.cancel(gameObject);
        Cr.SetActive(false);
        AudioManager.Play("Background");
    }

    public void LoadSecret() => UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSecret");    

}
