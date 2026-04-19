using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI-to-Physical bridge for spawning atoms.
/// Ensures scene hygiene by clearing the spawn zone before instantiation.
/// </summary>
public class AtomDispenser : MonoBehaviour
{
    #region System References
    [Header("Environment")]
    [Tooltip("Tracks atoms currently on the pad to prevent physics stacking.")]
    public SpawnZoneSensor spawnZoneSensor;

    [Tooltip("The designated spawn coordinate.")]
    public Transform spawnPoint;
    #endregion

    #region Prefabs
    [Header("Atom Prefabs")]
    public GameObject hydrogenPrefab;
    public GameObject oxygenPrefab;
    public GameObject carbonPrefab;
    public GameObject nitrogenPrefab;
    #endregion

    #region UI Components
    [Header("UI Controls")]
    public Button btnSpawnH;
    public Button btnSpawnO;
    public Button btnSpawnC;
    public Button btnSpawnN;
    #endregion

    #region Lifecycle
    private void Start()
    {
        // Event-driven architecture: code-based binding is more robust for Git/version control
        if (btnSpawnH != null) btnSpawnH.onClick.AddListener(() => ExecuteSpawn(hydrogenPrefab));
        if (btnSpawnO != null) btnSpawnO.onClick.AddListener(() => ExecuteSpawn(oxygenPrefab));
        if (btnSpawnC != null) btnSpawnC.onClick.AddListener(() => ExecuteSpawn(carbonPrefab));
        if (btnSpawnN != null) btnSpawnN.onClick.AddListener(() => ExecuteSpawn(nitrogenPrefab));
    }

    private void OnDestroy()
    {
        // Explicit teardown to prevent memory leaks and orphaned listeners
        if (btnSpawnH != null) btnSpawnH.onClick.RemoveAllListeners();
        if (btnSpawnO != null) btnSpawnO.onClick.RemoveAllListeners();
        if (btnSpawnC != null) btnSpawnC.onClick.RemoveAllListeners();
        if (btnSpawnN != null) btnSpawnN.onClick.RemoveAllListeners();
    }
    #endregion

    #region Internal Logic
    /// <summary>
    /// Centralized spawn logic. Clears existing pad contents before creating the new entity.
    /// </summary>
    /// <param name="prefab">The atom prefab to instantiate.</param>
    private void ExecuteSpawn(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"[Dispenser] Attempted to spawn a null prefab on {gameObject.name}");
            return;
        }

        // Cleanup: Ensures zero-frame physics stability by removing overlapping rigidbodies
        spawnZoneSensor.ClearAtoms();
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
    #endregion

    #region Legacy Public Wrappers
    // Kept for inspector compatibility or external script calls
    public void SpawnHydrogen() => ExecuteSpawn(hydrogenPrefab);
    public void SpawnOxygen() => ExecuteSpawn(oxygenPrefab);
    public void SpawnCarbon() => ExecuteSpawn(carbonPrefab);
    public void SpawnNitrogen() => ExecuteSpawn(nitrogenPrefab);
    #endregion
}