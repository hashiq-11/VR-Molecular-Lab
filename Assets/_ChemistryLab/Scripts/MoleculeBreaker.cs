using UnityEngine;

/// <summary>
/// Implements the Reset Mechanism by decomposing a molecule into its base atoms.
/// This allows for ingredient reuse within the lab environment.
/// </summary>
public class MoleculeBreaker : MonoBehaviour
{
    #region Configuration
    [Header("Raw Atoms to Spawn")]
    [Tooltip("The original atom prefabs required to reconstruct this molecule (e.g., 2H + 1O for H2O).")]
    public GameObject[] rawAtoms;
    #endregion

    #region Public API
    /// <summary>
    /// Spawns the constituent atoms with a slight offset and cleans up the molecule.
    /// Typically triggered by a VR 'Activate' event (e.g., pulling the trigger).
    /// </summary>
    public void BreakApart()
    {
        // --- 1. RECONSTRUCTION ---
        foreach (GameObject atomPrefab in rawAtoms)
        {
            if (atomPrefab == null) continue;

            // --- 2. PHYSICS SAFETY ---
            // Calculate a random offset to prevent atoms from spawning inside each other, 
            // which prevents aggressive physics 'explosions' in VR.
            Vector3 randomOffset = Random.insideUnitSphere * 0.15f;

            // Force upward/lateral bias to ensure atoms don't clip through the table surface.
            randomOffset.y = Mathf.Abs(randomOffset.y);

            Instantiate(atomPrefab, transform.position + randomOffset, Quaternion.identity);
        }

        // --- 3. CLEANUP ---
        // Immediate disposal of the complex molecule to optimize scene memory.
        Destroy(gameObject);
    }
    #endregion
}