using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using DG.Tweening;

/// <summary>
/// Controls the physical and interactive state of individual atoms in the VR space.
/// Handles local visual feedback (hover/grab) and delegates chemistry logic to the BondManager.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class AtomController : MonoBehaviour
{
    #region Serialized Fields
    [Header("Atom Identity")]
    [Tooltip("The ScriptableObject defining this atom's chemical properties.")]
    public AtomData atomData;

    [Header("State Management")]
    [Tooltip("Prevents race conditions by locking the atom during a bonding process.")]
    public bool isBonded = false;
    #endregion

    #region Private References & Variables
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private Renderer meshRenderer;
    private Vector3 originalScale;
    #endregion

    #region Lifecycle
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<Renderer>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // --- PHYSICS OPTIMIZATION ---
        // Damping is essential in VR to prevent objects from feeling 'floaty' 
        // or jittering endlessly when placed on surfaces.
        rb.linearDamping = 3f;
        rb.angularDamping = 3f;
    }

    private void Start()
    {
        originalScale = transform.localScale;

        // Initialize element-specific properties
        if (atomData != null)
        {
            gameObject.name = $"{atomData.elementName}_Atom";

            if (meshRenderer != null)
            {
                meshRenderer.material.color = atomData.atomColor;
            }
        }

        // --- UX / JUICE ---
        // Procedural entrance animation for a polished feel.
        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale, 0.6f).SetEase(Ease.OutBack);

        RegisterInteractableEvents();
    }

    private void OnDestroy()
    {
        // --- MEMORY SAFETY ---
        // Crucial: Clear all active tweens and event listeners to prevent 
        // memory leaks and NullReferenceExceptions during scene transitions.
        transform.DOKill();
        UnregisterInteractableEvents();
    }
    #endregion

    #region Interaction Callbacks
    private void RegisterInteractableEvents()
    {
        if (grabInteractable == null) return;

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.hoverEntered.AddListener(OnHoverEnter);
        grabInteractable.hoverExited.AddListener(OnHoverExit);
    }

    private void UnregisterInteractableEvents()
    {
        if (grabInteractable == null) return;

        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
        grabInteractable.hoverEntered.RemoveListener(OnHoverEnter);
        grabInteractable.hoverExited.RemoveListener(OnHoverExit);
    }

    private void OnGrab(SelectEnterEventArgs args) => transform.DOScale(originalScale * 1.1f, 0.1f);
    private void OnRelease(SelectExitEventArgs args) => transform.DOScale(originalScale, 0.1f);

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        // Only trigger if not held to avoid tween conflicts with the grab logic.
        if (!grabInteractable.isSelected)
            transform.DOScale(originalScale * 1.2f, 0.1f);
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        if (!grabInteractable.isSelected)
            transform.DOScale(originalScale, 0.1f);
    }
    #endregion

    #region Bonding Logic
    private void OnCollisionEnter(Collision collision)
    {
        // Guard Clause: Atom is already part of a reaction.
        if (isBonded) return;

        // Optimization: TryGetComponent avoids the overhead of traditional GetComponent.
        if (collision.gameObject.TryGetComponent(out AtomController otherAtom))
        {
            if (otherAtom.isBonded) return;

            // Intentional Interaction: Bonding only occurs if the player is actively 
            // manipulating one of the components, preventing accidental 'table-top' reactions.
            if (grabInteractable.isSelected)
            {
                // DELEGATION: Keeping logic centralized in the BondManager.
                BondManager.Instance.TryBond(this, otherAtom);
            }
        }
    }
    #endregion
}