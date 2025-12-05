using UnityEngine;
using UnityEngine.EventSystems;

public class hoverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseOver;
    public GameObject hoverShow;
    public blueprintScript blueprintScript;
    public folderScript folderScript;
    public bool isBlueprint;
    public bool isInnerFolder;
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        hoverShow.SetActive(true);
        if (isInnerFolder)
        {
            folderScript.onPutInFolder();
        }
        else
        {
            if (isBlueprint)
            {
                blueprintScript.checkMovePosition();
            }
            else
            {
                folderScript.checkMovePosition();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        hoverShow.SetActive(false);
        if (isInnerFolder)
        {
            folderScript.offPutInFolder();
        }
    }
}
