using UnityEngine;
using TMPro;

public class inputScript : MonoBehaviour
{
    public string inputText;
    public TMP_Text placeHolder;
    public void selectText()
    {
        placeHolder.text = string.Empty;
    }

    public void deselectText()
    {
        placeHolder.text = inputText;
    }
}
