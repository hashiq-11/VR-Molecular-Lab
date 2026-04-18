using UnityEngine;

public class MoleculeBreaker : MonoBehaviour
{
    [Header("Raw Atoms to Spawn")]
    [Tooltip("Drag the raw atom prefabs here (e.g., 2 Hydrogens, 1 Oxygen for Water)")]
    public GameObject[] rawAtoms;

    public void BreakApart()
    {
        // 1. Spawn the raw atoms around the molecule's current position
        foreach (GameObject atomPrefab in rawAtoms)
        {
            if (atomPrefab != null)
            {
                // Add a slight random offset so they don't spawn exactly inside each other and explode
                Vector3 randomOffset = Random.insideUnitSphere * 0.15f;
                randomOffset.y = Mathf.Abs(randomOffset.y); // Keep them bouncing upwards

                Instantiate(atomPrefab, transform.position + randomOffset, Quaternion.identity);
            }
        }

        // 2. (Optional) Play a break sound if you added the AudioManager
        // if (AudioManager.Instance != null) AudioManager.Instance.PlaySuccess(); 

        // 3. Destroy the completed molecule
        Destroy(gameObject);
    }
}