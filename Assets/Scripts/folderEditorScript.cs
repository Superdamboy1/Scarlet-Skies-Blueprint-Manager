using TMPro;
using UnityEngine;

public class folderEditorScript : MonoBehaviour
{
    public folderScript targetFolderScript;
    public TMP_InputField folderName;
    public string folderPath;
    public bool isNewFolder;
    public GameObject folderEditor;

    public void cancel()
    {
        folderEditor.SetActive(false);
        if (isNewFolder)
        {

            Destroy(targetFolderScript.gameObject);
        }
    }

    public void saveFolder()
    {
        targetFolderScript.saveFolder(folderPath + @"\" + fixName(folderName.text), isNewFolder);
    }

    public string fixName(string name)
    {
        name = name.Replace("\\", "");
        name = name.Replace("/", "");
        name = name.Replace(":", "");
        name = name.Replace("*", "");
        name = name.Replace("?", "");
        name = name.Replace("\"", "");
        name = name.Replace("<", "");
        name = name.Replace(">", "");
        name = name.Replace("|", "");
        name = name.Replace("\n", "");
        name = name.Replace("\r", "");
        return name;
    }
}
