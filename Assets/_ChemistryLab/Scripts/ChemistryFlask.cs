using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Manages atom collection through physical trigger volumes. 
/// Handles ingredient aggregation and delegates synthesis logic to the BondManager.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ChemistryFlask : MonoBehaviour
{
    #region Serialized Fields & State
    [Header("Configuration")]
    [Tooltip("The exact transform where the completed molecule should appear.")]
    public Transform spawnPoint;
    public Button synthesizeButton;

    [Header("Live State")]
    [Tooltip("Collection of atoms currently inside the flask volume.")]
    public List<AtomController> containedAtoms = new List<AtomController>();
    #endregion

    #region Lifecycle
    private void Start()
    {
        // Subscribe via code to ensure persistence across prefab iterations
        if (synthesizeButton != null)
        {
            synthesizeButton.onClick.AddListener(Synthesize);
        }
    }

    private void OnDestroy()
    {
        // Critical: Prevent memory leaks and orphaned listeners
        if (synthesizeButton != null)
        {
            synthesizeButton.onClick.RemoveListener(Synthesize);
        }
    }
    #endregion

    #region Physics Aggregation
    private void OnTriggerEnter(Collider other)
    {
        // TryGetComponent is non-allocating and faster than traditional GetComponent
        if (other.TryGetComponent(out AtomController atom))
        {
            // Defensive check to avoid duplicate entries from physics jitter
            if (!containedAtoms.Contains(atom))
            {
                containedAtoms.Add(atom);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out AtomController atom))
        {
            if (containedAtoms.Contains(atom))
            {
                containedAtoms.Remove(atom);
            }
        }
    }
    #endregion

    #region Core Logic
    /// <summary>
    /// Evaluates current contents and attempts to form a molecule via BondManager.
    /// </summary>
    public void Synthesize()
    {
        // Guard Clause: Minimum requirement for a chemical bond
        if (containedAtoms.Count < 2)
        {
            Debug.LogWarning("[Chemistry] Insufficient atoms for synthesis.");
            return;
        }

        // DELEGATION: Keeping the flask's responsibility limited to 'holding'
        bool success = BondManager.Instance.TryBondFlask(containedAtoms, spawnPoint.position);

        if (success)
        {
            // Ingredients are consumed on success; clear local references
            containedAtoms.Clear();
        }
    }
    #endregion
}