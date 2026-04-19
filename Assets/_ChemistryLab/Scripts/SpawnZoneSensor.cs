using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monitors a physical trigger volume to track and manage atoms currently on the spawn pad.
/// Ensures the environment is cleared before new spawns to maintain physics stability.
/// </summary>
[RequireComponent(typeof(Collider))]
public class SpawnZoneSensor : MonoBehaviour
{
    #region State
    /// <summary>
    /// Tracks all atoms currently within the sensor's trigger volume.
    /// </summary>
    public List<AtomController> AtomsInArea { get; private set; } = new List<AtomController>();
    #endregion

    #region Initialization
    private void Start()
    {
        // Enforce trigger state on the collider to detect overlaps without physical resistance.
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }
    #endregion

    #region Trigger Logic
    private void OnTriggerEnter(Collider other)
    {
        // Optimization: TryGetComponent is more efficient for high-frequency physics checks.
        if (other.TryGetComponent(out AtomController atom))
        {
            if (!AtomsInArea.Contains(atom))
            {
                AtomsInArea.Add(atom);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out AtomController atom))
        {
            if (AtomsInArea.Contains(atom))
            {
                AtomsInArea.Remove(atom);
            }
        }
    }
    #endregion

    #region Cleanup
    /// <summary>
    /// Deletes all atoms currently on the pad.
    /// Call this before spawning a new atom to prevent prefab overlapping and physics 'explosions'.
    /// </summary>
    public void ClearAtoms()
    {
        // Guard against any atoms that may have been destroyed externally (null check).
        AtomsInArea.RemoveAll(atom => atom == null);

        foreach (AtomController atom in AtomsInArea)
        {
            // Immediate cleanup to keep the scene hierarchy clean.
            Destroy(atom.gameObject);
        }

        AtomsInArea.Clear();
    }
    #endregion
}