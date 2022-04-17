using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Sprite Selected, UnSelected;
    public void SelectOrg()
    {
        PlayerPrefs.SetInt("SelectedSkin", 0);
    }

}
