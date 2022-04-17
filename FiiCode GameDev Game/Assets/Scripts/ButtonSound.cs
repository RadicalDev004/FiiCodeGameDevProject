using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    private void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PlaySound);
    }
    private void PlaySound()
    {
        AudioManager.Play("ButtonPress");
    }
}
