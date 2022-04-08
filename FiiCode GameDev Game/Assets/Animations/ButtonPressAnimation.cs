using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Pixelplacement;

public class ButtonPressAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool Cooldown = false;
    private readonly float time = 0.3f;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Cooldown) return;
        Cooldown = true;

        Tween.LocalScale(transform, new Vector3(0.75f, 0.75f, 0.75f) ,time, 0, Tween.EaseOutStrong);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Tween.LocalScale(transform, new Vector3(1f, 1f, 1f), time, 0, Tween.EaseOutStrong);

        Invoke(nameof(ResetCooldown), time);
    }

    private void ResetCooldown() => Cooldown = false;
}
