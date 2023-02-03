using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursor : MonoBehaviour
{
    public bool enable;

    void Awake()
    {
        //Set Cursor to not be visible
        Cursor.visible = enable;
    }
}
