using UnityEngine;

public class UnlockMouse : MonoBehaviour
{
    void Start()
    {
        if (!Cursor.visible)
        {
            Cursor.visible = true;
        }
        else
            return;
    }
}
