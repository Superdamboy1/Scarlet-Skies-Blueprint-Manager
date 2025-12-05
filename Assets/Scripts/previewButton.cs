#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(blueprintPreviewer))]
public class previewButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        blueprintPreviewer blueprintPreviewer = (blueprintPreviewer)target;

        if (GUILayout.Button("Preview Blueprint"))
        {
            blueprintPreviewer.createBlueprint();
        }

        if (GUILayout.Button("Clear Blueprint"))
        {
            blueprintPreviewer.clearBlueprint();
        }
    }
}
#endif
