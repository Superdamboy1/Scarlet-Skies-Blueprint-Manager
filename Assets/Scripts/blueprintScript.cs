using UnityEngine;
using TMPro;
using System.IO;
using System.Collections;
using System;
using UnityEngine.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using SFB;
using System.IO.Compression;
using System.ComponentModel.Design;

public class blueprintScript : MonoBehaviour
{
    [Header("Blueprint Information")]
    public string blueprintName;
    public string blueprint;
    public string blueprintFileReference;
    public string blueprintFolder;
    public int partCount;
    public string imagePath;
    [Header("Global References")]
    public GameObject blueprintEditor;
    public TMP_InputField blueprintNameInputText;
    public TMP_Text partCountText;
    public blueprintEditorScript blueprintEditorScript;
    public blueprintManagerScript blueprintManagerScript;
    [Header("Local References")]
    public GameObject bin;
    public GameObject questionMark;
    public TMP_Text blueprintNameText;
    public hoverScript upperHover;
    public hoverScript lowerHover;
    [Header("Other Variables")]
    public bool canDeleteBlueprint;
    public bool isMovingThisElement;
    [Header("Image Stuff")]
    public GameObject Camera;
    public RenderTexture renderTexture;
    public blueprintReader blueprintReader;
    public RawImage blueprintImage;

    public void copyBlueprint()
    {
        GUIUtility.systemCopyBuffer = blueprint;
    }

    public void editBlueprint()
    {
        blueprintEditor.SetActive(true);
        blueprintNameInputText.text = blueprintName;
        partCountText.text = "Parts:\n" + partCount;
        blueprintEditorScript.blueprint = blueprint;
        blueprintEditorScript.targetBlueprintScript = gameObject.GetComponent<blueprintScript>();
        blueprintEditorScript.blueprintFolder = blueprintFolder;
        blueprintEditorScript.isNewBlueprint = false;
    }

    public void createBlueprint()
    {
        blueprintEditor.SetActive(true);
        blueprintNameInputText.text = blueprintName;
        partCountText.text = "Parts:\n" + partCount;
        blueprintEditorScript.blueprint = blueprint;
        blueprintEditorScript.targetBlueprintScript = gameObject.GetComponent<blueprintScript>();
        blueprintEditorScript.blueprintFolder = blueprintFolder;
        blueprintEditorScript.isNewBlueprint = true;
    }

    public void deleteBlueprint()   
    {
        if (canDeleteBlueprint)
        {
            if (File.Exists(blueprintFileReference))
            {
                File.Delete(blueprintFileReference);
            }
            blueprintManagerScript.updateManifest(blueprintFolder);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(ConfirmDelete());
        }
    }

    IEnumerator ConfirmDelete()
    {
        canDeleteBlueprint = true;
        bin.SetActive(false);
        questionMark.SetActive(true);
        yield return new WaitForSeconds(5);
        bin.SetActive(true);
        questionMark.SetActive(false);
        canDeleteBlueprint = false;
    }

    public void saveBlueprint(string blueprintToSaveName, string blueprintToSave, int blueprintToSavePartCount, string newReference, string newImageReference)
    {
        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }

        blueprint = blueprintToSave;
        blueprintName = blueprintToSaveName;
        partCount = blueprintToSavePartCount;
        blueprintNameText.text = blueprintToSaveName;
        imagePath = newImageReference;
        UpdateImage();

        if (!Directory.Exists(blueprintFolder))
        {
            Directory.CreateDirectory(blueprintFolder);
        }

        if (File.Exists(blueprintFileReference))
        {
            File.Delete(blueprintFileReference);
        }

        using (StreamWriter writer = new StreamWriter(newReference))
        {
            writer.WriteLine("1");
            writer.WriteLine(blueprintName);
            writer.WriteLine(blueprint);
            writer.WriteLine(partCount);
            writer.WriteLine(imagePath);
        }
        blueprintManagerScript.updateManifest(blueprintFolder);

        blueprintEditor.SetActive(false);
    }

    public void preMove()
    {
        isMovingThisElement = true;
        blueprintManagerScript.preMove(gameObject, blueprintName, transform.GetSiblingIndex());
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

    public async Awaitable UpdateImage()
    {
        await Awaitable.MainThreadAsync();
        int count = blueprintManagerScript.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            DestroyImmediate(blueprintManagerScript.transform.GetChild(0).gameObject);
        }
        blueprintReader.buildBlueprint(blueprint, blueprintManagerScript.transform);
        Transform cameraTransform = blueprintReader.buildAndGetCamera(blueprint, blueprintManagerScript.transform);
        Camera.transform.position = cameraTransform.position;
        Camera.transform.rotation = cameraTransform.rotation;
        Camera.GetComponent<Camera>().Render();
        Texture2D image = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        RenderTexture.active = renderTexture;
        image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = null;
        byte[] imageBytes = image.EncodeToPNG();
        File.WriteAllBytes(imagePath, imageBytes);
        loadImage();
    }

    public void loadImage()
    {
        byte[] bytes = File.ReadAllBytes(imagePath);
        Texture2D image = new(1024, 1024, TextureFormat.RGBA32, 1, true);
        image.LoadImage(bytes);
        blueprintImage.texture = image;
    }

    public void exportBlueprint()
    {
        string savePath = StandaloneFileBrowser.SaveFilePanel("Export Folder", "", blueprintName, "bpx");
        File.Copy(blueprintFileReference, savePath);
    }
}
