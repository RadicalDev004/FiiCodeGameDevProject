using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Loading : MonoBehaviour
{
    public Slider Load;
    public int buffer;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Load = GetComponent<Slider>();

        Load.minValue = 0;
        Load.maxValue = 100;
        Load.value = 0;

        buffer = Random.Range(10, 75);

        StartCoroutine(DeBuffer());
        StartCoroutine(EndLoad());
    }

    private void FixedUpdate()
    {
        if (Load.value < Load.maxValue && Load.value != buffer)
        {
            Load.value += 1;
        }
    }

    private IEnumerator DeBuffer()
    {
        yield return new WaitUntil(() => Load.value == buffer);

        yield return new WaitForSecondsRealtime(Random.Range(0.3f, 1.5f));

        if (100 - buffer > 15) buffer = Random.Range(buffer + 1, 100);
        else buffer = 0;

        StartCoroutine(DeBuffer());
    }

    private IEnumerator EndLoad()
    {
        yield return new WaitUntil(() => Load.value == Load.maxValue);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
