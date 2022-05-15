using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPosAndScale : MonoBehaviour
{
    public GameObject Background;

    void Update()
    {
        
        GetComponent<RectTransform>().anchoredPosition = Background.GetComponent<RectTransform>().anchoredPosition;

        GetComponent<RectTransform>().localScale = Background.GetComponent<RectTransform>().sizeDelta/4500;
    }
}
