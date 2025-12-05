using UnityEngine;

public class blueprintPreviewer : MonoBehaviour
{
    public blueprintReader blueprintReader;
    public string blueprint;

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
}
