using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class blueprintPreviewer : MonoBehaviour
{
    public blueprintReader blueprintReader;
    public string blueprint;
    public GameObject Camera;
    public GameObject previewer;
    public RectTransform previewerTransform;
    public bool previewing;
    public Transform runtimeTransform;
    public RenderTexture RenderTexture;
    public TMP_Text blueprintNameText;
    public RawImage previewImage;
    public float camX;
    public float camY;
    public float zoomSensitivity;
    public Vector2 previousMouse;
    public Vector2 currentMouse;
    public Vector2 rotation;

    public void createBlueprint()
    {
        clearBlueprint();
        blueprintReader.buildAndGetCamera(blueprint, transform);
    }
    public void clearBlueprint()
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    public void clearBlueprint(Transform parent)
    {
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }
    }

    public void runtimePreviewBlueprint(string blueprintName, string blueptint, Transform parent)
    {
        runtimeTransform = parent.parent;
        parent.position = Vector3.zero;
        parent.rotation = Quaternion.identity;
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }
        RenderTexture = new RenderTexture((int)previewerTransform.rect.width, (int)previewerTransform.rect.height, 32);
        previewImage.texture = RenderTexture;
        Camera.GetComponent<Camera>().targetTexture = RenderTexture;

        returnInfo returnInfo = blueprintReader.buildRuntime(blueptint, parent);
        blueprintReader.buildOutline(blueptint, parent, Vector3.one);

        transform.GetChild(0).transform.position = returnInfo.centre * -1;
        Vector3 relMin = returnInfo.min - returnInfo.centre;
        Vector3 relMax = returnInfo.max - returnInfo.centre;
        
        camX = Mathf.Max(Math.Abs(relMin.x), Math.Abs(relMax.x), Math.Abs(relMin.z), Math.Abs(relMax.z)) * 1.5f;
        camY = Mathf.Max(Math.Abs(relMax.y), Math.Abs(relMin.y));

        Camera.transform.position = new Vector3(0f, 0f, Vector3.Magnitude(new Vector3(camX, camY)) * -1f);
        Camera.transform.LookAt(Vector3.zero);

        previousMouse = Mouse.current.position.ReadValue();

        Camera.GetComponent<Camera>().enabled = true;
        Camera.GetComponent<Camera>().fieldOfView = 90f;
        Camera.GetComponent<Camera>().Render();
        previewer.SetActive(true);
        blueprintNameText.text = blueprintName;
        previewing = true;
    }

    private void Update()
    {
        if (previewing && Mouse.current != null)
        {
            currentMouse = Mouse.current.position.ReadValue();
            Vector3 mousechange = currentMouse - previousMouse;
            if (Mouse.current.leftButton.isPressed)
            {
                runtimeTransform.Rotate(mousechange.y / Screen.height * 180f, mousechange.x / Screen.width * -720f, 0f, Space.World);
            }

            if (Mouse.current.rightButton.isPressed)
            {
                Vector3 move = new Vector3(mousechange.x * Camera.transform.position.z / 800, mousechange.y * Camera.transform.position.z / 800, 0);
                Camera.transform.position += move;
            }

            previousMouse = currentMouse;

            float scroll = Mouse.current.scroll.ReadValue().y;
            scroll *= zoomSensitivity;
            Vector3 position = Camera.transform.position + new Vector3(0f, 0f, scroll);
            position.z = Mathf.Clamp(position.z, Mathf.NegativeInfinity, -1f);
            Camera.transform.position = position;
            
        }
    }

    public void cancelPreview()
    {
        previewing = false;
        previewer.SetActive(false);
        Camera.GetComponent<Camera>().enabled = false;
        clearBlueprint(transform.GetChild(0));
        runtimeTransform.rotation = Quaternion.identity;
    }
}
