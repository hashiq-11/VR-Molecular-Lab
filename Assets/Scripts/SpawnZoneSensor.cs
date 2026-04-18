using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnZoneSensor : MonoBehaviour
{
    public List<AtomController> AtomsInArea { get; private set; } = new List<AtomController>();

    private void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
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

    public void ClearAtoms()
    {
        AtomsInArea.RemoveAll(atom => atom == null); 

        foreach (AtomController atom in AtomsInArea)
        {
            Destroy(atom.gameObject);
        }

        AtomsInArea.Clear();
    }
}