using UnityEngine;

public class MoleculeBreaker : MonoBehaviour
{
    [Header("Raw Atoms to Spawn")]
    [Tooltip("Drag the raw atom prefabs here (e.g., 2 Hydrogens, 1 Oxygen for Water)")]
    public GameObject[] rawAtoms;

    public void BreakApart()
    {
        foreach (GameObject atomPrefab in rawAtoms)
        {
            if (atomPrefab != null)
            {
                Vector3 randomOffset = Random.insideUnitSphere * 0.15f;
                randomOffset.y = Mathf.Abs(randomOffset.y); 

                Instantiate(atomPrefab, transform.position + randomOffset, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}