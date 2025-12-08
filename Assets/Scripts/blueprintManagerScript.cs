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

        loadOldVersions();
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
                Dictionary<string, GameObject> blueprintDictionary = new Dictionary<string, GameObject>();
                Dictionary<string, GameObject> folderDictionary = new Dictionary<string, GameObject>();
                Dictionary<string, int> manifestDictionary = new Dictionary<string, int>();
                List<string> manifest = new List<string>();

                if (getManifest(path) != null)
                {
                    manifest = getManifest(path);
                    for (int i = 0; i < manifest.Count; i++)
                    {
                        if (manifest[i] != "" && manifest[i] != null)
                        {
                            manifestDictionary.Add(manifest[i], i);
                        }
                    }
                }


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
                    folderScript.binTransform = binTransform;

                    folderScript.folderEditor = folderEditor;
                    folderScript.folderNameInputText = folderNameInputText;
                    folderScript.folderEditorScript = folderEditorScript;
                    folderScript.blueprintManagerScript = manager;
                    folderDictionary.Add(folderScript.folderName, duplicatedFolder);
                    if (manifestDictionary.ContainsKey("1" + folderScript.folderName))
                    {
                        duplicatedFolder.transform.SetSiblingIndex(manifestDictionary["1" + folderScript.folderName] + Convert.ToInt32(path != rootFolder));
                    }
                    if (topFolder != null)
                    {
                        topFolder.transform.SetAsFirstSibling();
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
                    blueprintScript.binTransform = binTransform;
                    blueprintScript.Camera = imageTaker;
                    blueprintScript.blueprintPreviewer = blueprintPreviewer;
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
                    if (blueprintDictionary.ContainsKey(blueprintScript.blueprintName))
                    {
                        int z = 2;
                        while (blueprintDictionary.ContainsKey(blueprintScript.blueprintName + z))
                        {
                            z++;
                        }
                        blueprintScript.blueprintName = blueprintScript.blueprintName + z;
                        blueprintScript.saveBlueprint(blueprintScript.blueprintName, blueprintScript.blueprint, blueprintScript.partCount, blueprintScript.blueprintFileReference, blueprintScript.imagePath);
                    }
                    blueprintScript.blueprintNameText.text = blueprintScript.blueprintName;
                    blueprintDictionary.Add(blueprintScript.blueprintName, duplicatedBlueprint);

                    if (manifestDictionary.ContainsKey("0" + blueprintScript.blueprintName))
                    {
                        duplicatedBlueprint.transform.SetSiblingIndex(manifestDictionary["0" + blueprintScript.blueprintName] + Convert.ToInt32(path != rootFolder));
                    }
                    if (topFolder != null)
                    {
                        topFolder.transform.SetAsFirstSibling();
                    }
                    await Awaitable.BackgroundThreadAsync();
                }
                await Awaitable.MainThreadAsync();

                for (int i = 0; i < manifest.Count; i++)
                {
                    if (manifest[i] != null)
                    {
                        if (manifest[i].Length >= 2)
                        {
                            if (manifest[i].Substring(0, 1) == "0")
                            {
                                blueprintDictionary[manifest[i].Substring(1)].transform.SetSiblingIndex(i + Convert.ToInt32(path != rootFolder));
                            }
                            if (manifest[i].Substring(0, 1) == "1")
                            {
                                folderDictionary[manifest[i].Substring(1)].transform.SetSiblingIndex(i + Convert.ToInt32(path != rootFolder));
                            }
                        }
                    }
                }

                updateManifest(path);
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
        string[] fileDescription = importPath.Split('.');
        string fileExtension = fileDescription[fileDescription.Length - 1];
        string[] filePath = importPath.Split('\\');

        List<string> manifest = getManifest(activeFolder);

        if (fileExtension == "bpx")
        {
            string name = "";
            using (StreamReader sr = new StreamReader(importPath))
            {
                if(sr.ReadLine() == "1")
                {
                    name = sr.ReadLine();
                }
            }
            string blueprintPath = activeFolder + "\\" + fixName(name);
            if (File.Exists(blueprintPath + ".bpx") || manifest.Contains("0" + name))
            {
                int z = 2;
                while (File.Exists(blueprintPath + z + ".bpx") || manifest.Contains("0" + name + z))
                {
                    z++;
                }
                blueprintPath = blueprintPath + z;
                name = name + z;
            }
            manifest.Add("0" + name);
            blueprintPath = blueprintPath + ".bpx";

            File.Copy(importPath, blueprintPath, true);
        }
        if (fileExtension == "zip")
        {
            string newPath = activeFolder + "\\" + Path.GetFileNameWithoutExtension(importPath);
            if (Directory.Exists(newPath))
            {
                int z = 2;
                while (Directory.Exists(newPath + z))
                {
                    z++;
                }
                newPath = newPath + z;
            }
            ZipFile.ExtractToDirectory(importPath, newPath, true);
        }
        if (fileExtension == "ssbp")
        {
            int x = 0;
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
                        }
                        if (x % 3 == 1)
                        {
                            string blueprintPath = activeFolder + "\\" + fixName(name);
                            if (File.Exists(blueprintPath + ".bpx") || manifest.Contains("0" + name))
                            {
                                int z = 2;
                                while (File.Exists(blueprintPath + z + ".bpx") || manifest.Contains("0" + name + z))
                                {
                                    z++;
                                }
                                blueprintPath = blueprintPath + z;
                                name = name + z;
                            }
                            manifest.Add("0" + name);
                            blueprintPath = blueprintPath + ".bpx";

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

        string manifestPath = activeFolder + "\\manifest.man";
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

        importPath = null;
        await LoadFolder(activeFolder);
        overWritePopup.SetActive(false);
    }
    public async void importCreateOverwrite()
    {
        string[] fileDescription = importPath.Split('.');
        string fileExtension = fileDescription[fileDescription.Length - 1];
        string[] filePath = importPath.Split('\\');

        List<string> manifest = getManifest(activeFolder);

        if (fileExtension == "bpx")
        {
            string name = "";
            using (StreamReader sr = new StreamReader(importPath))
            {
                if (sr.ReadLine() == "1")
                {
                    name = sr.ReadLine();
                }
            }
            string blueprintPath = activeFolder + "\\" + fixName(name);
            if (!manifest.Contains("0" + name))
            {
                manifest.Add("0" + name);
            }
            File.Copy(importPath, blueprintPath, true);
        }
        if (fileExtension == "zip")
        {
            ZipFile.ExtractToDirectory(importPath, activeFolder, true);
            List<string> newManifest = getManifest(activeFolder);
            for(int i = 0; i < newManifest.Count; i++)
            {
                if (!manifest.Contains(newManifest[i]))
                {
                    manifest.Add(newManifest[i]);
                }
            }
        }
        if (fileExtension == "ssbp")
        {
            int x = 0;
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
                        }
                        if (x % 3 == 1)
                        {
                            string blueprintPath = activeFolder + "\\" + fixName(name) + ".bpx";
                            if (File.Exists(blueprintPath))
                            {
                                File.Delete(blueprintPath);
                            }
                            if (!manifest.Contains("0" + name))
                            {
                                manifest.Add("0" + name);
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

        string manifestPath = activeFolder + "\\manifest.man";
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

        importPath = null;
        await LoadFolder(activeFolder);
        overWritePopup.SetActive(false);
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

            string path = rootFolder + "\\" + fixName(blueprintName);
            if (File.Exists(path + ".bpx") || manifest.Contains("0" + blueprintName))
            {
                int z = 2;
                while (File.Exists(path + z + ".bpx") || manifest.Contains("0" + blueprintName + z))
                {
                    z++;
                }
                path = path + z;
                blueprintName = blueprintName + z;
            }
            path = path + ".bpx";

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("1");
                writer.WriteLine(blueprintName);
                writer.WriteLine(blueprint);
                writer.WriteLine(partCount);
                writer.WriteLine(rootFolder + "\\" + fixName(blueprintName) + ".png");
            }
            manifest.Add("0" + blueprintName);
            y++;
        }

        PlayerPrefs.DeleteAll();


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
                    }
                    if (x % 3 == 1)
                    {
                        if (!Directory.Exists(rootFolder))
                        {
                            Directory.CreateDirectory(rootFolder);
                        }

                        string blueprintPath = rootFolder + "\\" + fixName(name);
                        if (File.Exists(blueprintPath + ".bpx") || manifest.Contains("0" + name))
                        {
                            int z = 2;
                            while (File.Exists(blueprintPath + z + ".bpx") || manifest.Contains("0" + name + z))
                            {
                                z++;
                            }
                            blueprintPath = blueprintPath + z;
                            name = name + z;
                        }
                        blueprintPath = blueprintPath + ".bpx";

                        using (StreamWriter writer = new StreamWriter(blueprintPath))
                        {
                            writer.WriteLine("1");
                            writer.WriteLine(name);
                            writer.WriteLine(line);
                            writer.WriteLine(blueprintReader.getPartCount(line));
                            writer.WriteLine(rootFolder + "\\" + fixName(name) + ".png");
                        }
                        manifest.Add("0" + name);
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
        using (StreamWriter writer = new StreamWriter(manifestPath))
        {
            for (int i = 0; i < content.childCount; i++)
            {
                if ((i != 0 && activeFolder != rootFolder) || activeFolder == rootFolder)
                {
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
            updateManifest(activeFolder);
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
                    canvasScaler.scaleFactor = zoom;
                    PlayerPrefs.SetFloat("zoom", zoom);
                }

                if (Keyboard.current.minusKey.wasPressedThisFrame)
                {
                    zoom /= 1.1f;
                    canvasScaler.scaleFactor = zoom;
                    PlayerPrefs.SetFloat("zoom", zoom);
                }

                if (Keyboard.current.digit0Key.wasPressedThisFrame)
                {
                    zoom = 1f;
                    canvasScaler.scaleFactor = zoom;
                    PlayerPrefs.SetFloat("zoom", zoom);
                }
            }
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
            File.Delete(images[i]);
        }
        LoadFolder(activeFolder);

        createMenu.SetActive(false);
    }

    public void closeOverWritePopup()
    {
        overWritePopup.SetActive(false);
    }
}