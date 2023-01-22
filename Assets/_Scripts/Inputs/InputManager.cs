using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    public static KeyCode FORWARD = KeyCode.UpArrow;
    public static KeyCode BACKWARD = KeyCode.DownArrow;
    public static KeyCode TURNRIGHT = KeyCode.RightArrow;
    public static KeyCode TURNLEFT = KeyCode.LeftArrow;


    public static KeyCode JUMP = KeyCode.A;
    public static KeyCode WALL = KeyCode.S;
    public static KeyCode INVISIBLE = KeyCode.D;

    public static string VERTICAL = "Vertical";
    public static string HORIZONTAL = "Horizontal";

    public static KeyCode PAUSE = KeyCode.F3;
    public static KeyCode FULLSCREEN = KeyCode.F4;
}
