using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Hides the cursor
        Cursor.visible = false;

        // Locks the cursor to the center of the game window
        //Cursor.lockState = CursorLockMode.Locked;
    }

}
