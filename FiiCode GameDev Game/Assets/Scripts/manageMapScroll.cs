using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class manageMapScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuBackgroundScroll.PauseScroll = 1;
        //Debug.Log("Stop Map Scroll");
        //Debug.LogWarning(MenuBackgroundScroll.PauseScroll);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuBackgroundScroll.PauseScroll = 0;
        //Debug.Log("Start Map Scroll");
    }
}
