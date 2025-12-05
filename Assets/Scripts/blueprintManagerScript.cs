using System;
using System.IO;
using TMPro;
using UnityEngine;
using SFB;
using System.IO.Compression;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.InputSystem;

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
    public List<GameObject> UIElementList;

    [Header("Image Stuff")]
    public GameObject imageTaker;

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
        loadOldVersions();
        preLoadFolder();
        await LoadFolder(rootFolder);
    }

    public void preLoadFolder()
    {
        //clear current view
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }

    public async Awaitable LoadFolder(string path)
    {
        await Awaitable.MainThreadAsync();

        if (Directory.Exists(path))
        {
            activeFolder = path;
            Dictionary<string, GameObject> blueprintDictionary = new Dictionary<string, GameObject>();
            Dictionary<string, GameObject> folderDictionary = new Dictionary<string, GameObject>();
            Dictionary<string, int> manifestDictionary = new Dictionary<string, int>();

            if (getManifest(path) != null)
            {
                List<string> manifest = getManifest(path);
                for (int i = 0; i < manifest.Count; i++)
                {
                    if (manifest[i] != "" && manifest[i] != null)
                    {
                        manifestDictionary.Add(manifest[i], i);
                    }
                }
            }

            //get folders in directory
            string[] directories = Directory.GetDirectories(path);
            foreach (string dir in directories)
            {
                await Awaitable.MainThreadAsync();
                GameObject duplicatedFolder = Instantiate(folderPrefab, content);
                folderScript folderScript = duplicatedFolder.GetComponent<folderScript>();
                string[] pathArray = dir.Split('\\');
                folderScript.folderName = pathArray[pathArray.Length - 1];
                folderScript.folderNameText.text = folderScript.folderName;
                folderScript.folderReference = dir;
                folderScript.folderFolder = path;

                folderScript.folderEditor = folderEditor;
                folderScript.folderNameInputText = folderNameInputText;
                folderScript.folderEditorScript = folderEditorScript;
                folderScript.blueprintManagerScript = manager;
                folderDictionary.Add(folderScript.folderName, duplicatedFolder);
                if (manifestDictionary.ContainsKey("1" + folderScript.folderName))
                {
                    duplicatedFolder.transform.SetSiblingIndex(manifestDictionary["1" + folderScript.folderName]);
                }
                await Awaitable.BackgroundThreadAsync();
            }

            //get blueprints in directory
            string[] blueprints = Directory.GetFiles(path, "*.bpx");
            foreach (string blueprint in blueprints)
            {
                await Awaitable.MainThreadAsync();
                GameObject duplicatedBlueprint = Instantiate(blueprintPrefab, content);
                blueprintScript blueprintScript = duplicatedBlueprint.GetComponent<blueprintScript>();
                using (StreamReader reader = new StreamReader(blueprint))
                {
                    string type = reader.ReadLine();
                    if (type == "1")
                    {
                        blueprintScript.blueprintFileReference = blueprint;
                        blueprintScript.blueprintFolder = path;
                        blueprintScript.blueprintName = reader.ReadLine();
                        blueprintScript.blueprintNameText.text = blueprintScript.blueprintName;
                        blueprintScript.blueprint = reader.ReadLine();
                        int partCount;
                        int.TryParse(reader.ReadLine(), out partCount);
                        blueprintScript.partCount = partCount;
                        blueprintScript.imagePath = reader.ReadLine();
                    }
                }

                blueprintScript.blueprintEditor = blueprintEditor;
                blueprintScript.blueprintNameInputText = blueprintNameInputText;
                blueprintScript.partCountText = partCountText;
                blueprintScript.blueprintEditorScript = blueprintEditorScript;
                blueprintScript.blueprintManagerScript = manager;
                blueprintScript.blueprintReader = blueprintReader;
                blueprintScript.Camera = imageTaker;
                if (blueprintScript.imagePath == "" || blueprintScript.imagePath == null || !File.Exists(blueprintScript.imagePath))
                {
                    blueprintScript.imagePath = path + "\\" + fixName(blueprintScript.blueprintName) + ".png";
                    await blueprintScript.UpdateImage();
                }
                else
                {
                    blueprintScript.loadImage();
                }
                blueprintDictionary.Add(blueprintScript.blueprintName, duplicatedBlueprint);
                if (manifestDictionary.ContainsKey("0" + blueprintScript.blueprintName))
                {
                    duplicatedBlueprint.transform.SetSiblingIndex(manifestDictionary["0" + blueprintScript.blueprintName]);
                }
                await Awaitable.BackgroundThreadAsync();
            }
            await Awaitable.MainThreadAsync();


            //add return if needed
            if (path != rootFolder)
            {
                GameObject duplicatedFolder = Instantiate(returnFolderPrefab, content);
                folderScript folderScript = duplicatedFolder.GetComponent<folderScript>();
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
                duplicatedFolder.transform.SetAsFirstSibling();
            }

            updateManifest(path);
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

    public async void importstuff()
    {
        ExtensionFilter[] extensions = new[]
        {
            new ExtensionFilter("Blueprint Files", "ssbp", "bpx", "zip")
        };
        string path = StandaloneFileBrowser.OpenFilePanel("Import Blueprints and Folders", "", extensions, false)[0];

        string[] fileDescription = path.Split('.');
        string fileExtension = fileDescription[fileDescription.Length - 1];
        string[] filePath = path.Split('\\');

        if (fileExtension == "bpx")
        {
            File.Copy(path, activeFolder + "\\" + filePath[filePath.Length - 1], true);
        }
        if (fileExtension == "zip")
        {
            ZipFile.ExtractToDirectory(path, activeFolder, true);
        }
        if (fileExtension == "ssbp")
        {
            int x = 0;
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    string name = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (x % 3 == 0)
                        {
                            name = line;
                        }
                        if (x % 3 == 1)
                        {
                            if (!Directory.Exists(rootFolder))
                            {
                                Directory.CreateDirectory(rootFolder);
                            }

                            string blueprintPath = rootFolder + "\\" + fixName(name) + ".bpx";
                            if (File.Exists(blueprintPath))
                            {
                                File.Delete(blueprintPath);
                            }

                            using (StreamWriter writer = new StreamWriter(blueprintPath))
                            {
                                writer.WriteLine("1");
                                writer.WriteLine(name);
                                writer.WriteLine(line);
                                writer.WriteLine(blueprintReader.getPartCount(line));
                            }
                        }
                        x++;
                    }
                }
            }
        }
        createMenu.SetActive(false);
        await LoadFolder(activeFolder);
    }

    public void loadOldVersions()
    {
        List<string> manifest = getManifest(rootFolder);

        int y = 0;
        while ((PlayerPrefs.GetString("blueprintName" + y) != "") || (PlayerPrefs.GetString("blueprint" + y)) != "")
        {
            string blueprint = PlayerPrefs.GetString("blueprint" + y);
            string blueprintName = PlayerPrefs.GetString("blueprintName" + y);
            int partCount = blueprintReader.getPartCount(blueprint);

            if (!Directory.Exists(rootFolder))
            {
                Directory.CreateDirectory(rootFolder);
            }

            string path = rootFolder + "\\" + fixName(blueprintName) + ".bpx";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("1");
                writer.WriteLine(blueprintName);
                writer.WriteLine(blueprint);
                writer.WriteLine(partCount);
            }
            manifest.Add("0" + blueprintName);
            y++;
        }


        int x = 0;
        string importPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Scarlet Skies Blueprint Manager\Blueprints.bbsp";
        if (File.Exists(importPath))
        {
            using (StreamReader sr = new StreamReader(importPath))
            {
                string line;
                string name = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (x % 3 == 0)
                    {
                        name = line;
                        manifest.Add("0" + line);
                    }
                    if (x % 3 == 1)
                    {
                        if (!Directory.Exists(rootFolder))
                        {
                            Directory.CreateDirectory(rootFolder);
                        }

                        string blueprintPath = rootFolder + "\\" + fixName(name) + ".bpx";
                        if (File.Exists(blueprintPath))
                        {
                            File.Delete(blueprintPath);
                        }

                        using (StreamWriter writer = new StreamWriter(blueprintPath))
                        {
                            writer.WriteLine("1");
                            writer.WriteLine(name);
                            writer.WriteLine(line);
                            writer.WriteLine(blueprintReader.getPartCount(line));
                            writer.WriteLine(rootFolder + "\\" + fixName(name) + ".png");
                        }
                    }
                    x++;
                }
            }
            File.Delete(importPath);
        }

        if (!Directory.Exists(rootFolder))
        {
            Directory.CreateDirectory(rootFolder);
        }

        string manifestPath = rootFolder + "\\manifest.man";
        if (File.Exists(manifestPath))
        {
            File.Delete(manifestPath);
        }

        using (StreamWriter writer = new StreamWriter(manifestPath))
        {
            for (int i = 0; i < manifest.Count; i++)
            {
                writer.WriteLine(manifest[i]);
            }
        }
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

    public List<string> getManifest(string folder)
    {
        List<string> manifest = new List<string>();
        string manifestReference = folder + "\\" + "manifest.man";
        if (File.Exists(manifestReference))
        {
            using (StreamReader reader = new StreamReader(manifestReference))
            {
                string line;
                manifest.Add(line = reader.ReadLine());
                while ((line = reader.ReadLine()) != null)
                {
                    manifest.Add(line);
                }
            }
        }
        return manifest;
    }

    public void updateManifest(string path)
    {
        string manifestPath = path + "\\manifest.man";
        UIElementList.Clear();
        using (StreamWriter writer = new StreamWriter(manifestPath))
        {
            for (int i = 0; i < content.childCount; i++)
            {
                if ((i != 0 && activeFolder != rootFolder) || activeFolder == rootFolder)
                {
                    UIElementList.Add(content.GetChild(i).gameObject);
                    if (content.GetChild(i).GetComponent<blueprintScript>() != null)
                    {
                        string name = content.GetChild(i).GetComponent<blueprintScript>().blueprintName;
                        content.GetChild(i).GetComponent<blueprintScript>().isMovingThisElement = false;
                        writer.WriteLine("0" + name);
                    }
                    if (content.GetChild(i).GetComponent<folderScript>() != null)
                    {
                        string name = content.GetChild(i).GetComponent<folderScript>().folderName;
                        content.GetChild(i).GetComponent<folderScript>().isMovingThisElement = false;
                        writer.WriteLine("1" + name);
                    }
                }
            }
        }
    }

    public void preMove(GameObject uiElement, string label, int index)
    {
        movingSomething = true;
        movingIndex = index;
        itemBeingMoved = uiElement;
        for (int i = 0; i < UIElementList.Count; i++)
        {
            UIElementList[i].transform.GetChild(1).gameObject.SetActive(true);
            UIElementList[i].transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            UIElementList[i].transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        }
        moveLabel.gameObject.SetActive(true);
        moveLabel.text = label;
    }

    public void Update()
    {
        if (movingSomething && (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame))
        {
            movingSomething = false;
            for (int i = 0; i < UIElementList.Count; i++)
            {
                UIElementList[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            itemBeingMoved.transform.SetSiblingIndex(movePosition);
            moveLabel.gameObject.SetActive(false);
            updateManifest(activeFolder);
        }

        if (Keyboard.current != null && Keyboard.current.f11Key.wasPressedThisFrame)
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }

    public void search()
    {
        string searchText = searchInput.text;
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if ((i != 0 && activeFolder != rootFolder) || activeFolder == rootFolder)
            {
                if (content.GetChild(i).GetComponent<blueprintScript>() != null)
                {
                    content.GetChild(i).gameObject.SetActive(content.GetChild(i).GetComponent<blueprintScript>().blueprintName.Contains(searchText));
                }
                if (content.GetChild(i).GetComponent<folderScript>() != null)
                {

                    content.GetChild(i).gameObject.SetActive(content.GetChild(i).GetComponent<folderScript>().folderName.Contains(searchText));
                }
            }
        }
    }

    public void exportActiveFolder()
    {
        string savePath = StandaloneFileBrowser.SaveFilePanel("Export Folder", "", activeFolder, "zip");
        ZipFile.CreateFromDirectory(activeFolder, savePath);
    }
}
