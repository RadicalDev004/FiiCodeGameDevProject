using UnityEngine;
[ExecuteInEditMode]
public class LevelbuttonLayout : MonoBehaviour
{
    public RectTransform[] buttons;
    public float dista = 335;
    public float angle;

    private void OnEnable()
    {
        Debug.Log("Arrange");
        for (int i = 1; i < buttons.Length; i++)
        {
            buttons[i].localPosition = buttons[i - 1].localPosition + GetPosition(-45 + -(i + i * 0.1f) * angle-i, 335);
        }
    }

    Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(-Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }
}
