using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string headerText;
    public string contentText;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Instance.Show(contentText, headerText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Instance.Hide();
    }
}
