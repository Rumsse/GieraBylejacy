using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTextureNormal; 
    public Texture2D cursorTextureClicked; 
    public Vector2 hotSpot = Vector2.zero; 
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        Cursor.SetCursor(cursorTextureNormal, hotSpot, cursorMode);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0)) 
        {
            Cursor.SetCursor(cursorTextureClicked, hotSpot, cursorMode);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorTextureNormal, hotSpot, cursorMode);
        }
    }

}
