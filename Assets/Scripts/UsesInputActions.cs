using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Should be inherited by any classes which use the InputActions class.
/// Use protected override void Awake2(), OnDisable2(), or OnEnable2()
/// in place of their original versions if you need to use those methods.
/// </summary>
public abstract class UsesInputActions : MonoBehaviour
{
    protected InputActions inputActions { get; private set; }

    /// <summary>
    /// Don't override this! Use Awake2() if you want more stuff to happen on Awake() time
    /// </summary>
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inputActions = new InputActions();
        Awake2();
    }

    /// <summary>
    /// Don't override this! Use OnEnable2() if you want more stuff to happen on Awake() time
    /// </summary>
    private void OnEnable()
    {
        inputActions.Enable();
        OnEnable2();
    }

    /// <summary>
    /// Don't override this! Use OnDisable2() if you want more stuff to happen on Awake() time
    /// </summary>
    private void OnDisable()
    {
        inputActions.Disable();
        OnDisable2();
    }

    /// <summary>
    /// For use in place of Awake() in classes which override UsesInputActions. Use
    /// keywords "protected override void"
    /// </summary>
    protected virtual void Awake2()
    {
        // Override this in a subclass if you
        // want extra stuff to happen there at Awake() time
    }

    /// <summary>
    /// For use in place of OnEnable() in classes which override UsesInputActions. Use
    /// keywords "protected override void"
    /// </summary>
    protected virtual void OnEnable2()
    {
        // Override this in a subclass if you
        // want extra stuff to happen there at Awake() time
    }

    /// <summary>
    /// For use in place of OnDisable() in classes which override UsesInputActions. Use
    /// keywords "protected override void"
    /// </summary>
    protected virtual void OnDisable2()
    {
        // Override this in a subclass if you
        // want extra stuff to happen there at Awake() time
    }

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
