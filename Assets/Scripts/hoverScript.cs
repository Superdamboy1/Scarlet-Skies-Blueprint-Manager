using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class hoverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseOver;
    public GameObject hoverShow;
    public blueprintScript blueprintScript;
    public folderScript folderScript;
    public bool isBlueprint;
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        hoverShow.SetActive(true);
        if (isBlueprint)
        {
            blueprintScript.checkMovePosition();
        }
        else
        {
            folderScript.checkMovePosition();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        hoverShow.SetActive(false);
    }
}
