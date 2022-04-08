using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class Star : MonoBehaviour
{
    private float rot = 0;
    private float x = 200;

    public void GetCollected(float time)
    {
        Tween.Position(transform, transform.position + 5 * Vector3.up, time, 0, Tween.EaseOut);
        x = 500;
        StartCoroutine(WaitAndDestroy(time));

    }

    IEnumerator WaitAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }


    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, rot, 0);
        rot += x * Time.deltaTime;
    }
}
