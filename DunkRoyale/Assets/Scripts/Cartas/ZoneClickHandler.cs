using UnityEngine;
using UnityEngine.InputSystem;

public class ZoneClickHandler : MonoBehaviour
{
    void OnMouseDown()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(screenPos);
        clickPos.z = 0f;
        GridManager.Instance.OnZoneClicked(clickPos);
    }
}