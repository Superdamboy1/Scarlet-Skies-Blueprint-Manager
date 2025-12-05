using UnityEngine;
using UnityEngine.EventSystems;

public class moveUIGrabScript : MonoBehaviour, IPointerDownHandler
{
    public blueprintScript blueprintScript;
    public folderScript folderScript;
    public bool isBlueprint;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isBlueprint)
        {
            blueprintScript.preMove();
        }
        else
        {
            folderScript.preMove();
        }
    }
}
