using System;
using System.IO;
using TMPro;
using UnityEngine;
using SFB;
using System.IO.Compression;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class blueprintManagerScript : MonoBehaviour
{
    public Transform content;
    public string activeFolder;
    public blueprintManagerScript manager;
    public string rootFolder;
    public GameObject createMenu;
    public blueprintReader blueprintReader;
    public TMP_Text moveLabel;
    public int movePosition;
    public bool movingSomething;
    public int movingIndex;
    public GameObject itemBeingMoved;
    public TMP_InputField searchInput;
    public CanvasScaler canvasScaler;
    public float zoom;
    public bool isOverFolder;
    public string folderPath;
    public bool dontMove;
    public Transform binTransform;
    public GameObject loadingObject;
    public GameObject topFolder;
    public string importPath;
    public GameObject overWritePopup;
    public bool loadingSomething;

    public TMP_Text FPSText;

    [Header("Image Stuff")]
    public GameObject imageTaker;
    public blueprintPreviewer blueprintPreviewer;

    [Header("Blueprints")]
    public GameObject blueprintPrefab;

    public GameObject blueprintEditor;
    public TMP_InputField blueprintNameInputText;
    public TMP_Text partCountText;
    public blueprintEditorScript blueprintEditorScript;

    [Header("Folders")]
    public GameObject folderPrefab;
    public GameObject returnFolderPrefab;

    public GameObject folderEditor;
    public TMP_InputField folderNameInputText;
    public folderEditorScript folderEditorScript;


    private async void Start()
    {
        rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Scarlet Skies Blueprint Manager";
        activeFolder = rootFolder;
        if (PlayerPrefs.HasKey("zoom"))
        {
            zoom = PlayerPrefs.GetFloat("zoom");
        }
        else
        {
            zoom = 1f;
        }
        canvasScaler.scaleFactor = zoom;


        //loadOldVersions();
        await LoadFolder(rootFolder);
    }

    public async Awaitable LoadFolder(string path)
    {
        await Awaitable.MainThreadAsync();

        if (!loadingSomething)
        {
            loadingSomething = true;
            if (Directory.Exists(path))
            {
                //clear current view
                for (int i = 0; i < content.transform.childCount; i++)
                {
                    Destroy(content.transform.GetChild(i).gameObject);
                }
                activeFolder = path;


                //add return if needed
                if (path != rootFolder)
                {
                    topFolder = Instantiate(returnFolderPrefab, content);
                    folderScript folderScript = topFolder.GetComponent<folderScript>();
                    string targetDirectory = Directory.GetParent(path).ToString();
                    string[] pathArray = targetDirectory.Split('\\');
                    if (targetDirectory == rootFolder)
                    {
                        folderScript.folderNameText.text = "Return to Home";
                    }
                    else
                    {
                        folderScript.folderNameText.text = "Return to " + pathArray[pathArray.Length - 1];
                    }
                    folderScript.folderReference = targetDirectory;
                    folderScript.blueprintManagerScript = manager;
                    topFolder.transform.SetAsFirstSibling();
                }

                //get manifest
                List<manifestItem> manifest = getManifest(rootFolder);

                if (manifest != null)
                {
                    foreach (manifestItem item in manifest)
                    {
                        await Awaitable.MainThreadAsync();

                        //load blueprint
                        if (item.type == 0)
                        {
                            GameObject duplicatedBlueprint = Instantiate(blueprintPrefab, content);
                            blueprintScript blueprintScript = duplicatedBlueprint.GetComponent<blueprintScript>();
                            using (StreamReader reader = new StreamReader(item.reference))
                            {
                                string type = reader.ReadLine();
                                if (type == "2")
                                {
                                    blueprintScript.blueprintName = reader.ReadLine();
                                    blueprintScript.blueprint = reader.ReadLine();
                                    int partCount = 0;
                                    int.TryParse(reader.ReadLine(), out partCount);
                                    blueprintScript.partCount = partCount;
                                    blueprintScript.imagePath = reader.ReadLine();
                                }

                                int i = 0;
                                string line;
                                previousBlueprint tempBlueprint = new();
                                while ((line = reader.ReadLine()) != null)
                                {
                                    if (i % 2 == 0)
                                    {
                                        tempBlueprint.saveDate = line;
                                    }
                                    if (i % 2 == 1)
                                    {
                                        tempBlueprint.blueprint = line;
                                        blueprintScript.previousBlueprints.Add(tempBlueprint);
                                    }
                                    i++;
                                }
                            }

                            //set static and external variables
                            blueprintScript.blueprintFileReference = item.reference;
                            blueprintScript.blueprintFolder = path;
                            blueprintScript.blueprintEditor = blueprintEditor;
                            blueprintScript.blueprintNameInputText = blueprintNameInputText;
                            blueprintScript.partCountText = partCountText;
                            blueprintScript.blueprintEditorScript = blueprintEditorScript;
                            blueprintScript.blueprintManagerScript = manager;
                            blueprintScript.blueprintReader = blueprintReader;
                            blueprintScript.binTransform = binTransform;
                            blueprintScript.Camera = imageTaker;
                            blueprintScript.blueprintPreviewer = blueprintPreviewer;
                            blueprintScript.blueprintNameText.text = blueprintScript.blueprintName;

                            //create or update image
                            if (blueprintScript.imagePath == "" || blueprintScript.imagePath == null || !File.Exists(blueprintScript.imagePath) || blueprintScript.imagePath != path + "\\" + fixName(blueprintScript.blueprintName) + ".png")
                            {
                                if (File.Exists(blueprintScript.imagePath))
                                {
                                    File.Delete(blueprintScript.imagePath);
                                }
                                blueprintScript.imagePath = path + "\\" + fixName(blueprintScript.blueprintName) + ".png";
                                blueprintScript.saveBlueprint(blueprintScript.blueprintName, blueprintScript.blueprint, blueprintScript.partCount, blueprintScript.blueprintFileReference, blueprintScript.imagePath);
                            }
                            else
                            {
                                blueprintScript.loadImage();
                            }
                        }


                        //load Folder
                        if(item.type == 1)
                        {
                            GameObject duplicatedFolder = Instantiate(folderPrefab, content);
                            folderScript folderScript = duplicatedFolder.GetComponent<folderScript>();

                            //set static and external variables
                            folderScript.folderName = Path.GetDirectoryName(item.reference);
                            folderScript.folderNameText.text = folderScript.folderName;
                            folderScript.folderReference = item.reference;
                            folderScript.folderFolder = path;
                            folderScript.binTransform = binTransform;
                            folderScript.folderEditor = folderEditor;
                            folderScript.folderNameInputText = folderNameInputText;
                            folderScript.folderEditorScript = folderEditorScript;
                            folderScript.blueprintManagerScript = manager;

                            //load image
                            folderScript.loadImage();
                        }

                        //set the return as the top most item
                        if (topFolder != null)
                        {
                            topFolder.transform.SetAsFirstSibling();
                        }

                        await Awaitable.BackgroundThreadAsync();
                    }
                }
            }
            loadingSomething = false;
        }
    }

    public void toggleCreateMenu()
    {
        createMenu.SetActive(!createMenu.activeSelf);
    }

    public void createBlueprint()
    {
        GameObject newBlueprint = Instantiate(blueprintPrefab, content);
        blueprintScript blueprintScript = newBlueprint.GetComponent<blueprintScript>();
        blueprintScript.blueprintFolder = activeFolder;

        blueprintScript.blueprintEditor = blueprintEditor;
        blueprintScript.blueprintNameInputText = blueprintNameInputText;
        blueprintScript.partCountText = partCountText;
        blueprintScript.blueprintEditorScript = blueprintEditorScript;
        blueprintScript.blueprintManagerScript = manager;
        blueprintScript.blueprintReader = blueprintReader;
        blueprintScript.Camera = imageTaker;
        blueprintScript.blueprintPreviewer = blueprintPreviewer;

        blueprintScript.createBlueprint();
        createMenu.SetActive(false);
    }

    public void createFolder()
    {
        GameObject duplicatedFolder = Instantiate(folderPrefab, content);
        folderScript folderScript = duplicatedFolder.GetComponent<folderScript>();
        folderScript.folderFolder = activeFolder;

        folderScript.folderEditor = folderEditor;
        folderScript.folderNameInputText = folderNameInputText;
        folderScript.folderEditorScript = folderEditorScript;
        folderScript.blueprintManagerScript = manager;

        folderScript.createFolder();
        createMenu.SetActive(false);
    }

    public void importstuff()
    {
        ExtensionFilter[] extensions = new[]
        {
            new ExtensionFilter("Blueprint Files", "ssbp", "bpx", "zip")
        };
        string[] path = StandaloneFileBrowser.OpenFilePanel("Import Blueprints and Folders", "", extensions, false);
        if (path.Length > 0)
        {
            importPath = path[0];
            overWritePopup.SetActive(true);
        }
        createMenu.SetActive(false);
    }
    public async void importCreateNew()
    {


        importPath = null;
        await LoadFolder(activeFolder);
        overWritePopup.SetActive(false);
    }
    public async void importCreateOverwrite()
    {


        importPath = null;
        await LoadFolder(activeFolder);
        overWritePopup.SetActive(false);
    }

    public void loadOldVersions()
    {

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

    public List<manifestItem> getManifest(string folder)
    {
        List<manifestItem> manifest = new List<manifestItem>();
        string manifestReference = folder + "\\" + "manifest2.man";
        if (File.Exists(manifestReference))
        {
            using (StreamReader reader = new StreamReader(manifestReference))
            {
                string line;
                line = reader.ReadLine();
                if (line == "1")
                {
                    int i = 0;
                    manifestItem tempItem = new manifestItem();
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (i % 2 == 0)
                        {
                            int type = 0;
                            int.TryParse(line, out type);
                            tempItem.type = type;
                        }
                        if (i % 2 == 1)
                        {
                            tempItem.reference = line;
                            manifest.Add(tempItem);
                        }
                        i++;
                    }
                }
            }
        }
        return manifest;
    }

    public void updateManifast(string path)
    {
        //update manifest
    }

    public void preMove(GameObject uiElement, string label, int index)
    {
        movingSomething = true;
        movingIndex = index;
        itemBeingMoved = uiElement;
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i).GetComponent<blueprintScript>() != null)
            {
                content.GetChild(i).GetChild(1).gameObject.SetActive(true);
                content.GetChild(i).GetChild(1).GetChild(2).gameObject.SetActive(false);
                content.GetChild(i).GetChild(1).GetChild(3).gameObject.SetActive(false);
            }
            if (content.GetChild(i).GetComponent<folderScript>() != null)
            {
                content.GetChild(i).GetChild(1).gameObject.SetActive(true);
                content.GetChild(i).GetChild(1).GetChild(3).gameObject.SetActive(false);
                content.GetChild(i).GetChild(1).GetChild(4).gameObject.SetActive(false);
                content.GetChild(i).GetChild(1).GetChild(5).gameObject.SetActive(false);
            }
        }
        moveLabel.gameObject.SetActive(true);
        moveLabel.text = label;
    }

    public void Update()
    {
        if (movingSomething && (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame))
        {
            movingSomething = false;
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
            if (isOverFolder && !dontMove)
            {
                isOverFolder = false;
                if (itemBeingMoved.GetComponent<blueprintScript>() != null)
                {
                    blueprintScript reference = itemBeingMoved.GetComponent<blueprintScript>();
                    reference.saveBlueprint(reference.blueprintName, reference.blueprint, reference.partCount, folderPath + "\\" + fixName(reference.blueprintName) + ".bpx", folderPath + "\\" + fixName(reference.blueprintName) + ".png");
                }
                if (itemBeingMoved.GetComponent<folderScript>() != null)
                {
                    folderScript reference = itemBeingMoved.GetComponent<folderScript>();
                    reference.saveFolder(folderPath + "\\" + reference.folderName, false, folderPath);
                }
                DestroyImmediate(itemBeingMoved);
            }
            else
            {
                itemBeingMoved.transform.SetSiblingIndex(movePosition);
            }
            updateManifast(activeFolder);
            moveLabel.gameObject.SetActive(false);
            dontMove = false;
        }

        if (Keyboard.current != null)
        {
            if (Keyboard.current.ctrlKey.isPressed)
            {
                if (Keyboard.current.equalsKey.wasPressedThisFrame)
                {
                    zoom *= 1.1f;
                    PlayerPrefs.SetFloat("zoom", zoom);
                }

                if (Keyboard.current.minusKey.wasPressedThisFrame)
                {
                    zoom /= 1.1f;
                    PlayerPrefs.SetFloat("zoom", zoom);
                }

                if (Keyboard.current.digit0Key.wasPressedThisFrame)
                {
                    zoom = 1f;
                    PlayerPrefs.SetFloat("zoom", zoom);
                }
            }
        }

        canvasScaler.scaleFactor = zoom * (Screen.width / 1920f);

        FPSText.text = (1f / Time.deltaTime).ToString();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 10;
        }
    }

    public void search()
    {
        string searchText = searchInput.text.ToLower();
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if ((i != 0 && activeFolder != rootFolder) || activeFolder == rootFolder)
            {
                if (content.GetChild(i).GetComponent<blueprintScript>() != null)
                {
                    content.GetChild(i).gameObject.SetActive(content.GetChild(i).GetComponent<blueprintScript>().blueprintName.ToLower().Contains(searchText));
                }
                if (content.GetChild(i).GetComponent<folderScript>() != null)
                {
                    content.GetChild(i).gameObject.SetActive(content.GetChild(i).GetComponent<folderScript>().folderName.ToLower().Contains(searchText));
                }
            }
        }
    }

    public void exportActiveFolder()
    {
        string savePath = StandaloneFileBrowser.SaveFilePanel("Export Folder", "", activeFolder, "zip");
        ZipFile.CreateFromDirectory(activeFolder, savePath);
    }

    public void resetImages()
    {
        List<string> images = new List<string>();
        images.AddRange(Directory.GetFiles(rootFolder, "*.png", SearchOption.AllDirectories));
        for (int i = 0; i < images.Count; i++)
        {
            if (!images[i].EndsWith("Thumbnail.png"))
            {
                File.Delete(images[i]);
            }
        }
        LoadFolder(activeFolder);

        createMenu.SetActive(false);
    }

    public void closeOverWritePopup()
    {
        overWritePopup.SetActive(false);
    }
}

[System.Serializable]
public class manifestItem
{
    public int type;
    public string reference;
}

public class previousBlueprint
{
    public string saveDate;
    public string blueprint;
}

public class blueprintInfo
{
    public string name;
    public string blueprint;
}