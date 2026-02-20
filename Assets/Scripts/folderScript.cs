using UnityEngine;
using TMPro;
using System.IO;
using System.Collections;
using SFB;
using System.IO.Compression;
using System;
using UnityEngine.UI;

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
    public Transform binTransform;
    [Header("Local References")]
    public GameObject bin;
    public GameObject questionMark;
    public TMP_Text folderNameText;
    public hoverScript upperHover;
    public hoverScript lowerHover;
    public RawImage folderImage;
    public Texture defaultThumbnail;
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
            transform.SetParent(binTransform);
            blueprintManagerScript.updateManifast(folderFolder);
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

    public void saveFolder(string folderToSave, bool isNewFolder, string folderToSaveFolder)
    {
        if (isNewFolder)
        {
            if (Directory.Exists(folderToSave))
            {
                throw new Exception("Folder With Same Name Already Exists In Current Directory");
            }
            else
            {
                Directory.CreateDirectory(folderToSave);
            }
        }
        else
        {
            if (folderToSave != folderReference)
            {
                if (Directory.Exists(folderReference))
                {
                    if (Directory.Exists(folderToSave))
                    {
                        throw new Exception("Folder With Same Name Already Exists In Current Directory");
                    }
                    else
                    {
                        Directory.Move(folderReference, folderToSave);
                    }
                }
                else
                {
                    if (Directory.Exists(folderToSave))
                    {
                        throw new Exception("Folder With Same Name Already Exists In Current Directory");
                    }
                    else
                    {
                        Directory.CreateDirectory(folderToSave);
                    }
                }
            }
        }
        
        folderReference = folderToSave;
        folderFolder = folderToSaveFolder;
        string[] path = folderToSave.Split('\\');
        folderName = path[path.Length - 1];
        folderNameText.text = folderName;
        blueprintManagerScript.updateManifast(folderFolder);

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

    public void onPutInFolder()
    {
        blueprintManagerScript.isOverFolder = true;
        blueprintManagerScript.folderPath = folderReference;
        if (isMovingThisElement)
        {
            blueprintManagerScript.dontMove = true;
        }
    }

    public void offPutInFolder()
    {
        blueprintManagerScript.isOverFolder = false;
        if (isMovingThisElement)
        {
            blueprintManagerScript.dontMove = false;
        }
    }

    public void exportFolder()
    {
        string savePath = StandaloneFileBrowser.SaveFilePanel("Export Folder", "", folderName, "zip");
        ZipFile.CreateFromDirectory(folderReference, savePath);
    }

    public void loadImage()
    {
        if (File.Exists(folderReference + "\\Thumbnail.png"))
        {
            byte[] bytes = File.ReadAllBytes(folderReference + "\\Thumbnail.png");
            Texture2D image = new(1024, 1024, TextureFormat.RGBA32, 1, false);
            image.LoadImage(bytes);
            folderImage.texture = image;
        }
        else
        {
            folderImage.texture = defaultThumbnail;
        }
    }

    public void setThumbnail()
    {
        ExtensionFilter[] extensions = new[]
        {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg", "exr")
        };
        string path = StandaloneFileBrowser.OpenFilePanel("Select Thumbnail", folderReference, extensions, false)[0];
        File.Copy(path, folderReference + "\\Thumbnail.png", true);
        loadImage();
    }

    public void resetThumbnail()
    {
        if (File.Exists(folderReference + "\\Thumbnail.png"))
        {
            File.Delete(folderReference + "\\Thumbnail.png");
            loadImage();
        }
    }
}
