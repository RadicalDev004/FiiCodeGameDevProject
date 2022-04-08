using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int Level;
    public int Stars = 2;

    public bool HasCableStar = true;
    public bool HasChestStar = false;

    private void Awake()
    {
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
        if(!HasCableStar) Stars--;
        if(HasChestStar) Stars++;

        PlayerPrefs.SetInt("Level", Level + 1);

        //SceneManager.LoadScene("Level");
    }
}
