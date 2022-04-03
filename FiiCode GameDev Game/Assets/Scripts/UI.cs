using RadicalKit;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pixelplacement;

public class UI : MonoBehaviour
{
    public Image Background, EndScreen;
    private GameManager Manager;


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



    public void EndGameUI()
    {
        Background.gameObject.SetActive(true);

        Tween.Color(Background, new Color(0, 0, 0, 0.3f), 0.5f, 0, Tween.EaseInOut);
        Tween.LocalScale(EndScreen.GetComponent<RectTransform>(), Vector3.one, 0.5f, 0, Tween.EaseInOut);
    }

    private void PrepareUI()
    {
        Background.color = Change.ColorA(Background.color, 0);
        Background.gameObject.SetActive(false);

        EndScreen.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void ChangeSceneGeneric(string name)
    {
        SceneManager.LoadScene(name);
    }
}
