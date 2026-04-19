using UnityEngine;
using UnityEngine.InputSystem;


public class RayToggleManager : MonoBehaviour
{
    [Header("Ray Interactor GameObjects")]
    public GameObject leftRayObject;
    public GameObject rightRayObject;

    [Header("Input Actions (Drag from Inspector)")]
    [Tooltip("The action used to turn on the Left Ray (e.g., Left Primary Button)")]
    public InputActionReference leftToggleAction;

    [Tooltip("The action used to turn on the Right Ray (e.g., Right Primary Button)")]
    public InputActionReference rightToggleAction;

    void OnEnable()
    {
        // Subscribe the Left Hand
        if (leftToggleAction != null)
        {
            leftToggleAction.action.started += ctx => ToggleSpecificComponents(leftRayObject, true);
            leftToggleAction.action.canceled += ctx => ToggleSpecificComponents(leftRayObject, false);
        }

        // Subscribe the Right Hand
        if (rightToggleAction != null)
        {
            rightToggleAction.action.started += ctx => ToggleSpecificComponents(rightRayObject, true);
            rightToggleAction.action.canceled += ctx => ToggleSpecificComponents(rightRayObject, false);
        }

        // Start with rays off
        ToggleSpecificComponents(leftRayObject, false);
        ToggleSpecificComponents(rightRayObject, false);
    }

    void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks!
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

    private void ToggleSpecificComponents(GameObject controllerObject, bool isOn)
    {
        if (controllerObject == null) return;

        var rayInteractor = controllerObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        if (rayInteractor != null) rayInteractor.enabled = isOn;

        var lineVisual = controllerObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual>();
        if (lineVisual != null) lineVisual.enabled = isOn;

        var lineRenderer = controllerObject.GetComponent<LineRenderer>();
        if (lineRenderer != null) lineRenderer.enabled = isOn;
    }
}