using RadicalKit;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        LeanTween.color(Background.GetComponent<RectTransform>(), new Color32(0, 0, 0, 85), 0.25f);

        LeanTween.scale(EndScreen.GetComponent<RectTransform>(), Vector3.one, 0.25f);
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
