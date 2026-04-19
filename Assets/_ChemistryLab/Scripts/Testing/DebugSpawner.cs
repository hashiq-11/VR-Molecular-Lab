using UnityEngine;

public class DebugSpawner : MonoBehaviour
{
    [Header("Testing Setup")]
    [Tooltip("Where should the molecule appear?")]
    public Transform spawnPoint;

    [Header("Completed Prefabs to Test")]
    public GameObject waterPrefab;
    public GameObject hydrogenPrefab;
    public GameObject oxygenPrefab;
    public GameObject nitrogenPrefab;
    public GameObject ammoniaPrefab;
    public GameObject carbonDioxidePrefab;
    public GameObject methanePrefab;

    // A helper method to keep our spawner logic perfectly clean
    private void Spawn(GameObject prefab, string moleculeName)
    {
        if (prefab != null && spawnPoint != null)
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            Debug.Log($"[Debug] Spawned {moleculeName} for testing.");
        }
        else
        {
            Debug.LogWarning($"[Debug] Missing prefab or spawn point for {moleculeName}!");
        }
    }

    // ==========================================
    // RIGHT-CLICK CONTEXT MENUS FOR TESTING
    // ==========================================

    [ContextMenu("Spawn: Water (H2O)")]
    public void SpawnWater() => Spawn(waterPrefab, "Water");

    [ContextMenu("Spawn: Hydrogen Gas (H2)")]
    public void SpawnHydrogen() => Spawn(hydrogenPrefab, "Hydrogen Gas");

    [ContextMenu("Spawn: Oxygen Gas (O2)")]
    public void SpawnOxygen() => Spawn(oxygenPrefab, "Oxygen Gas");

    [ContextMenu("Spawn: Nitrogen Gas (N2)")]
    public void SpawnNitrogen() => Spawn(nitrogenPrefab, "Nitrogen Gas");

    [ContextMenu("Spawn: Ammonia (NH3)")]
    public void SpawnAmmonia() => Spawn(ammoniaPrefab, "Ammonia");

    [ContextMenu("Spawn: Carbon Dioxide (CO2)")]
    public void SpawnCO2() => Spawn(carbonDioxidePrefab, "Carbon Dioxide");

    [ContextMenu("Spawn: Methane (CH4)")]
    public void SpawnMethane() => Spawn(methanePrefab, "Methane");
}