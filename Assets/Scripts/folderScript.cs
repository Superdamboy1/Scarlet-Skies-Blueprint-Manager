using UnityEngine;
using TMPro;
using System.IO;
using System.Collections;
using SFB;
using System.IO.Compression;

public class folderScript : MonoBehaviour
{
    [Header("Folder Information")]
    public string folderName;
    public string folderReference;
    public string folderFolder;
    [Header("Global References")]
    public GameObject folderEditor;
    public TMP_InputField folderNameInputText;
    public folderEditorScript folderEditorScript;
    public blueprintManagerScript blueprintManagerScript;
    [Header("Local References")]
    public GameObject bin;
    public GameObject questionMark;
    public TMP_Text folderNameText;
    public hoverScript upperHover;
    public hoverScript lowerHover;
    [Header("Other Variables")]
    public bool canDeleteFolder;
    public bool isMovingThisElement;

    public void editFolder()
    {
        folderEditor.SetActive(true);
        folderNameInputText.text = folderName;
        folderEditorScript.targetFolderScript = gameObject.GetComponent<folderScript>();
        folderEditorScript.folderPath = folderFolder;
        folderEditorScript.isNewFolder = false;
    }

    public void createFolder()
    {
        folderEditor.SetActive(true);
        folderNameInputText.text = folderName;
        folderEditorScript.targetFolderScript = gameObject.GetComponent<folderScript>();
        folderEditorScript.folderPath = folderFolder;
        folderEditorScript.isNewFolder = true;
    }

    public void deleteFolder()
    {
        if (canDeleteFolder)
        {
            if (Directory.Exists(folderReference))
            {
                Directory.Delete(folderReference, true);
            }
            blueprintManagerScript.updateManifest(folderFolder);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(ConfirmDelete());
        }
    }

    IEnumerator ConfirmDelete()
    {
        canDeleteFolder = true;
        bin.SetActive(false);
        questionMark.SetActive(true);
        yield return new WaitForSeconds(5);
        bin.SetActive(true);
        questionMark.SetActive(false);
        canDeleteFolder = false;
    }

    public void saveFolder(string folderToSave, bool isNewFolder)
    {
        if (isNewFolder)
        {
            Directory.CreateDirectory(folderToSave);
        }
        else
        {
            if (Directory.Exists(folderReference))
            {
                if (!Directory.Exists(folderToSave))
                {
                    Directory.Move(folderReference, folderToSave);
                }
            }
            else
            {
                Directory.CreateDirectory(folderToSave);

            }
        }
        
        folderReference = folderToSave;
        string[] path = folderToSave.Split('\\');
        folderName = path[path.Length - 1];
        folderNameText.text = folderName;
        blueprintManagerScript.updateManifest(folderFolder);

        folderEditor.SetActive(false);
    }

    public async void openFolder()
    {
        await blueprintManagerScript.LoadFolder(folderReference);
    }

    public void preMove()
    {
        isMovingThisElement = true;
        blueprintManagerScript.preMove(gameObject, folderName, transform.GetSiblingIndex());
    }

    public void checkMovePosition()
    {
        if (isMovingThisElement)
        {
            if (upperHover.mouseOver || lowerHover.mouseOver)
            {
                blueprintManagerScript.movePosition = transform.GetSiblingIndex();
            }
        }
        else
        {
            if (transform.GetSiblingIndex() > blueprintManagerScript.movingIndex)
            {
                if (upperHover.mouseOver)
                {
                    blueprintManagerScript.movePosition = transform.GetSiblingIndex() - 1;
                }

                if (lowerHover.mouseOver)
                {
                    blueprintManagerScript.movePosition = transform.GetSiblingIndex();
                }
            }
            else
            {
                if (upperHover.mouseOver)
                {
                    blueprintManagerScript.movePosition = transform.GetSiblingIndex();
                }

                if (lowerHover.mouseOver)
                {
                    blueprintManagerScript.movePosition = transform.GetSiblingIndex() + 1;
                }
            }
        }
    }

    public void exportFolder()
    {
        string savePath = StandaloneFileBrowser.SaveFilePanel("Export Folder", "", folderName, "zip");
        ZipFile.CreateFromDirectory(folderReference, savePath);
    }
}
