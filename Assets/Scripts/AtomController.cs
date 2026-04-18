using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class AtomController : MonoBehaviour
{
    [Header("Atom Identity")]
    public AtomData atomData;

    [Header("State")]
    public bool isBonded = false;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private Renderer meshRenderer;

    private Vector3 originalScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<Renderer>();

        grabInteractable = GetComponent<XRGrabInteractable>();

        rb.linearDamping = 3f;  
        rb.angularDamping = 3f; 
    }

    void Start()
    {
        originalScale = transform.localScale;

        if (atomData != null)
        {
            gameObject.name = atomData.elementName + "_Atom";

            if (meshRenderer != null)
            {
                meshRenderer.material.color = atomData.atomColor;
            }
        }

        // DOTween pop-in animation
        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale, 0.6f).SetEase(Ease.OutBack);

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        transform.DOScale(originalScale * 1.1f, 0.1f);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        transform.DOScale(originalScale, 0.1f);
    }

    private void OnDestroy()
    {
        transform.DOKill();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isBonded) return;

        if (collision.gameObject.TryGetComponent(out AtomController otherAtom))
        {
            if (otherAtom.isBonded) return;

            if (grabInteractable.isSelected)
            {
                BondManager.Instance.TryBond(this, otherAtom);
            }
        }
    }

}