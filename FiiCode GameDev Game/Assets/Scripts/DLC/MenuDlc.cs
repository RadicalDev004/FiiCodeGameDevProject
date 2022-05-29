using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Pixelplacement;

public class MenuDlc : MonoBehaviour
{
    public Image NewLevelsOnline;
    public bool AnimationCooldown;
    public Image Load;

    private void Awake()
    {
        NewLevelsOnline.gameObject.SetActive(false);
    }

    public void OpenMakeYourLevel()
    {
        AudioManager.Play("EnterGame");
        StartCoroutine(LoadScene("LevelMaker"));
    }
    public void OpenSearchLevels()
    {
        AudioManager.Play("OpenPanel");
        OpenTab(NewLevelsOnline);
    }
    public void CloseSearchLevels()
    {
        AudioManager.Play("ClosePanel");
        CloseTab(NewLevelsOnline);
    }



    private void OpenTab(Image image)
    {
        if (AnimationCooldown) return;
        NewLevelsOnline.gameObject.SetActive(true);
        AnimationCooldown = true;

        Tween.LocalPosition(image.GetComponent<RectTransform>(), Vector3.zero, 0.5f, 0, Tween.EaseOut);
    }
    private void CloseTab(Image image)
    {
        Tween.LocalPosition(image.GetComponent<RectTransform>(), new Vector3(0, 1000, 0) , 0.5f, 0, Tween.EaseOut);
        Invoke(nameof(ResetAnimationCooldown), 0.5f);
    }
    private void ResetAnimationCooldown() { AnimationCooldown = false; NewLevelsOnline.gameObject.SetActive(false); }

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
