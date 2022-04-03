using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
[ExecuteInEditMode]
public class TweenTester : MonoBehaviour
{
    private void OnEnable()
    {
        Tween.Position(transform, transform.position - new Vector3(0, 5, 0), 1, 1, Tween.EaseIn);
    }
}
