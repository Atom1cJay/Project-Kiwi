using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UsesMouseInput : UsesInputActions
{
    /// <summary>
    /// Gives an appropriately inverted form of the mouse input using the old (smoothed) input system.
    /// </summary>
    protected Vector2 GetOldMouseInput()
    {
        bool invertedX = inputActions.Camera.HorizontalRotate.processors.Contains("Invert");
        bool invertedY = inputActions.Camera.VerticalRotate.processors.Contains("Invert");
        float mouseX = invertedX ? -Input.GetAxis("Mouse X") : Input.GetAxis("Mouse X");
        float mouseY = invertedY ? -Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y");
        return new Vector2(mouseX, mouseY);
    }
}
