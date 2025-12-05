using TMPro;
using UnityEngine;

public class blueprintEditorScript : MonoBehaviour
{
    public blueprintScript targetBlueprintScript;
    public TMP_InputField blueprintName;
    public string blueprint;
    public string blueprintFolder;
    public int partCount;
    public bool isNewBlueprint;
    public blueprintReader blueprintReader;
    public GameObject blueprintEditor;
    public TMP_Text partCountText;

    public void copyBlueprint()
    {
        blueprint = GUIUtility.systemCopyBuffer.Replace("\n", "").Replace("\r", "");
        partCount = blueprintReader.getPartCount(blueprint);
        partCountText.text = "Parts:\n" + partCount;
    }

    public void copySecondHalfBlueprint()
    {
        blueprint = blueprint + GUIUtility.systemCopyBuffer.Replace("\n", "").Replace("\r", "");
        partCount = blueprintReader.getPartCount(blueprint);
        partCountText.text = "Parts:\n" + partCount;
    }

    public void cancel()
    {
        blueprintEditor.SetActive(false);
        if (isNewBlueprint )
        {
           Destroy(targetBlueprintScript.gameObject);
        }
    }

    public async void saveBlueprint()
    {
        targetBlueprintScript.saveBlueprint(fixName(blueprintName.text), blueprint, blueprintReader.getPartCount(blueprint), blueprintFolder + "\\" + fixName(blueprintName.text) + ".bpx", blueprintFolder + "\\" + fixName(blueprintName.text) + ".png");
        await targetBlueprintScript.UpdateImage();
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
