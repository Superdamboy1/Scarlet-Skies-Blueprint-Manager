using UnityEngine;
using UnityEngine.EventSystems;

public class previewHoverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverShow;
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverShow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverShow.SetActive(false);
    }
}