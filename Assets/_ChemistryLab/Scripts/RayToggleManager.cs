using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the active state of XR Ray Interactors based on user input.
/// This ensures a clean UI experience by only showing laser pointers when requested.
/// </summary>
public class RayToggleManager : MonoBehaviour
{
    #region References
    [Header("Ray Interactor GameObjects")]
    [Tooltip("The GameObject containing the XR Ray Interactor for the left hand.")]
    public GameObject leftRayObject;

    [Tooltip("The GameObject containing the XR Ray Interactor for the right hand.")]
    public GameObject rightRayObject;

    [Header("Input Actions")]
    public InputActionReference leftToggleAction;
    public InputActionReference rightToggleAction;
    #endregion

    #region Lifecycle
    private void OnEnable()
    {
        // --- Event Subscription ---
        // Subscribing to started/canceled events allows for a "hold-to-show" mechanic
        if (leftToggleAction != null)
        {
            leftToggleAction.action.started += ctx => ToggleSpecificComponents(leftRayObject, true);
            leftToggleAction.action.canceled += ctx => ToggleSpecificComponents(leftRayObject, false);
        }

        if (rightToggleAction != null)
        {
            rightToggleAction.action.started += ctx => ToggleSpecificComponents(rightRayObject, true);
            rightToggleAction.action.canceled += ctx => ToggleSpecificComponents(rightRayObject, false);
        }

        // Initialize state: Rays should be hidden by default for a clutter-free start
        ToggleSpecificComponents(leftRayObject, false);
        ToggleSpecificComponents(rightRayObject, false);
    }

    private void OnDisable()
    {
        // --- Memory Management ---
        // Crucial: Always unsubscribe from Input System actions to prevent memory leaks and dangling references
        if (leftToggleAction != null)
        {
            leftToggleAction.action.started -= ctx => ToggleSpecificComponents(leftRayObject, true);
            leftToggleAction.action.canceled -= ctx => ToggleSpecificComponents(leftRayObject, false);
        }

        if (rightToggleAction != null)
        {
            rightToggleAction.action.started -= ctx => ToggleSpecificComponents(rightRayObject, true);
            rightToggleAction.action.canceled -= ctx => ToggleSpecificComponents(rightRayObject, false);
        }
    }
    #endregion

    #region Logic
    /// <summary>
    /// Selectively enables/disables interactor components. 
    /// This method preserves the GameObject's transform tracking while hiding visual/functional rays.
    /// </summary>
    private void ToggleSpecificComponents(GameObject controllerObject, bool isOn)
    {
        if (controllerObject == null) return;

        // Using GetComponent is acceptable here as this is an event-driven toggle, not a per-frame Update check
        var rayInteractor = controllerObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        if (rayInteractor != null) rayInteractor.enabled = isOn;

        var lineVisual = controllerObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual>();
        if (lineVisual != null) lineVisual.enabled = isOn;

        var lineRenderer = controllerObject.GetComponent<LineRenderer>();
        if (lineRenderer != null) lineRenderer.enabled = isOn;
    }
    #endregion
}