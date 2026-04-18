using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class AnimateHandOnInput : MonoBehaviour
{
    [SerializeField] private InputActionProperty _triggerAction;
    [SerializeField] private InputActionProperty _gripAction;

    private Animator _handAnimator;
    private static readonly int TriggerHash = Animator.StringToHash("Trigger");
    private static readonly int GripHash = Animator.StringToHash("Grip");

    private void Start()
    {
        _handAnimator = GetComponent<Animator>();

        if (_triggerAction.action == null || _gripAction.action == null)
        {
            Debug.LogError($"Input Actions missing on {gameObject.name}", this);
            enabled = false;
        }
    }

    private void Update()
    {
        _handAnimator.SetFloat(TriggerHash, _triggerAction.action.ReadValue<float>());
        _handAnimator.SetFloat(GripHash, _gripAction.action.ReadValue<float>());
    }
}