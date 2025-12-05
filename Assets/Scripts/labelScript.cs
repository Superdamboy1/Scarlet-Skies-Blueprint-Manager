using UnityEngine;
using UnityEngine.InputSystem;

public class labelScript : MonoBehaviour
{
    void Update()
    {
        transform.position = Mouse.current.position.ReadValue();
    }
}
