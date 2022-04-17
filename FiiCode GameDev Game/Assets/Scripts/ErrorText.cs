using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pixelplacement;

public class ErrorText : MonoBehaviour
{
    public GameObject errorText;
    private static ErrorText Instance;
    private bool Cooldown = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Display(string text)
    {
        if (Cooldown) return;
        Cooldown = true;
        Invoke(nameof(ResetCooldown), 1);

        GameObject err = Instantiate(errorText, transform);

        err.SetActive(true);
        err.GetComponent<RectTransform>().anchoredPosition = errorText.GetComponent<RectTransform>().anchoredPosition;

        Debug.Log(err.GetComponent<RectTransform>().anchoredPosition);
        err.GetComponent<TMP_Text>().text = text.ToUpper();

        StartCoroutine(ErrorAnimation(err.GetComponent<TMP_Text>(), 1.5f));
    }

    public static void DisplayError(string text)
    {
        
        Instance.Display(text);
    }

    private IEnumerator ErrorAnimation(TMP_Text error, float time)
    {
        LeanTween.move(error.GetComponent<RectTransform>(), new Vector3(0, 100, 0), time);      
        
        yield return new WaitForSecondsRealtime(time+1.5f);

        Tween.Color(error.GetComponent<TMP_Text>(), new Color32(255, 0, 0, 0), 0.5f, 0);

        yield return new WaitForSecondsRealtime(0.5f);

        DestroyError(error);
    }
    private void DestroyError(TMP_Text error) => Destroy(error.gameObject);
    private void ResetCooldown() => Cooldown = false;

}
