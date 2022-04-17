using UnityEngine;
[ExecuteInEditMode]
public class DottedLines : MonoBehaviour
{
    public GameObject line;
    public RectTransform[] UiLevels;

    private void OnEnable()
    {
        for (int i = 0; i < UiLevels.Length; i++)
        {
            Vector2 firstPos = UiLevels[i].anchoredPosition;
            Vector2 secondPos = UiLevels[i + 1].anchoredPosition;

            Vector2 desiredPos = (firstPos + secondPos) / 2;
            GameObject newLine = Instantiate(line, transform);
            newLine.GetComponent<RectTransform>().anchoredPosition = desiredPos;
            newLine.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -45 + 15 * i);
        }
    }
}
