using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Direct bridge between the XR Input System and Hand Animator.
/// Optimized for low-latency visual feedback on Quest/Mobile VR hardware.
/// </summary>
[RequireComponent(typeof(Animator))]
public class AnimateHandOnInput : MonoBehaviour
{
    #region Input References
    [Header("Input Action Properties")]
    [Tooltip("Maps to the index trigger for pinching/pointing animations.")]
    [SerializeField] private InputActionProperty _triggerAction;

    [Tooltip("Maps to the grip button for fist/grabbing animations.")]
    [SerializeField] private InputActionProperty _gripAction;
    #endregion

    #region Private State & Caching
    private Animator _handAnimator;

    // --- PERFORMANCE OPTIMIZATION ---
    // Caching hashes avoids per-frame string-to-int conversion in the Animator.
    private static readonly int TriggerHash = Animator.StringToHash("Trigger");
    private static readonly int GripHash = Animator.StringToHash("Grip");
    #endregion

    #region Lifecycle
    private void Start()
    {
        _handAnimator = GetComponent<Animator>();

        // --- DEFENSIVE PROGRAMMING ---
        // Prevents spamming NullReferenceExceptions if the developer forgets to assign actions.
        if (_triggerAction.action == null || _gripAction.action == null)
        {
            Debug.LogError($"[XR Architecture] Input Actions missing on {gameObject.name}. Script disabled.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        UpdateAnimationState();
    }
    #endregion

    #region Internal Logic
    /// <summary>
    /// Reads analog input values (0.0 to 1.0) and applies them to the Hand Blend Tree.
    /// </summary>
    private void UpdateAnimationState()
    {
        // ReadValue<float> handles analog trigger pressure for smooth transitions.
        _handAnimator.SetFloat(TriggerHash, _triggerAction.action.ReadValue<float>());
        _handAnimator.SetFloat(GripHash, _gripAction.action.ReadValue<float>());
    }
    #endregion
}