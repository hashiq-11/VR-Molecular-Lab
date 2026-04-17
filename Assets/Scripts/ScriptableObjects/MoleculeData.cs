using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AtomRequirement
{
    public AtomData atomType;
    public int requiredCount;
}

[CreateAssetMenu(fileName = "NewMolecule", menuName = "ChemistryLab/Molecule Data")]
public class MoleculeData : ScriptableObject
{
    [Header("Molecule Info")]
    public string moleculeName; // e.g., "Water"
    public string chemicalFormula; // e.g., "H2O"

    [Header("Recipe")]
    public List<AtomRequirement> requiredAtoms;

    [Header("Result")]
    public GameObject completedPrefab; // The VR object to spawn when successfully combined
}
