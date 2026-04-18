using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class ChemistryFlask : MonoBehaviour
{
    [Header("Flask State")]
    public List<AtomController> containedAtoms = new List<AtomController>();

    [Header("UI Reference")]
    public Button synthesizeButton;

    [Header("Spawning")]
    [Tooltip("Where the completed molecule should appear")]
    public Transform spawnPoint;

    private void Start()
    {
        if (synthesizeButton != null)
        {
            synthesizeButton.onClick.AddListener(Synthesize);
        }
    }

    private void OnDestroy()
    {
        if (synthesizeButton != null)
        {
            synthesizeButton.onClick.RemoveListener(Synthesize);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // When an atom drops into the flask, add it to our list
        if (other.TryGetComponent(out AtomController atom))
        {
            if (!containedAtoms.Contains(atom))
            {
                containedAtoms.Add(atom);
                Debug.Log($"Added {atom.atomData.elementName}. Total in flask: {containedAtoms.Count}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the player takes an atom out, remove it from the list
        if (other.TryGetComponent(out AtomController atom))
        {
            if (containedAtoms.Contains(atom))
            {
                containedAtoms.Remove(atom);
                Debug.Log($"Removed {atom.atomData.elementName} from flask.");
            }
        }
    }

    // You will link this method to a VR UI Button!
    public void Synthesize()
    {
        if (containedAtoms.Count < 2)
        {
            Debug.LogWarning("Need at least 2 atoms to synthesize!");
            return;
        }

        // Ask the BondManager to validate the entire flask
        bool success = BondManager.Instance.TryBondFlask(containedAtoms, spawnPoint.position);

        if (success)
        {
            // If successful, the BondManager destroyed the atoms, so we clear our list
            containedAtoms.Clear();
        }
    }
}