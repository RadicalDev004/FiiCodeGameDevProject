using UnityEngine;

public class DottedLine : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Level") >= transform.GetSiblingIndex() + 2)
        {
            GetComponent<UnityEngine.UI.Image>().color = new Color32(0, 72, 248, 255);
        }
        else
        {
            GetComponent<UnityEngine.UI.Image>().color = new Color32(244, 0, 27, 255);
        }
    }
}
