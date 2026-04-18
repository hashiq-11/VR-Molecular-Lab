using UnityEngine;
using UnityEngine.UI;

public class AtomDispenser : MonoBehaviour
{
    [Header("System References")]
    [Tooltip("Drag the physical Spawn Pad GameObject here")]
    public SpawnZoneSensor spawnZoneSensor;
    public Transform spawnPoint;

    [Header("Atom Prefabs")]
    public GameObject hydrogenPrefab;
    public GameObject oxygenPrefab;
    public GameObject carbonPrefab;
    public GameObject nitrogenPrefab;

    [Header("UI Buttons")]
    public Button btnSpawnH;
    public Button btnSpawnO;
    public Button btnSpawnC;
    public Button btnSpawnN;

    private void Start()
    {
        if (btnSpawnH != null) btnSpawnH.onClick.AddListener(SpawnHydrogen);
        if (btnSpawnO != null) btnSpawnO.onClick.AddListener(SpawnOxygen);
        if (btnSpawnC != null) btnSpawnC.onClick.AddListener(SpawnCarbon);
        if (btnSpawnN != null) btnSpawnN.onClick.AddListener(SpawnNitrogen);
    }

    private void OnDestroy()
    {
        if (btnSpawnH != null) btnSpawnH.onClick.RemoveListener(SpawnHydrogen);
        if (btnSpawnO != null) btnSpawnO.onClick.RemoveListener(SpawnOxygen);
        if (btnSpawnC != null) btnSpawnC.onClick.RemoveListener(SpawnCarbon);
        if (btnSpawnN != null) btnSpawnN.onClick.RemoveListener(SpawnNitrogen);
    }

    public void SpawnHydrogen()
    {
        spawnZoneSensor.ClearAtoms(); // Tell the physical pad to clear itself
        Instantiate(hydrogenPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void SpawnOxygen()
    {
        spawnZoneSensor.ClearAtoms();
        Instantiate(oxygenPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void SpawnCarbon()
    {
        spawnZoneSensor.ClearAtoms();
        Instantiate(carbonPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void SpawnNitrogen()
    {
        spawnZoneSensor.ClearAtoms();
        Instantiate(nitrogenPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}