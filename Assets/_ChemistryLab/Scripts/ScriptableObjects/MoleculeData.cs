using System.Collections.Generic;
using UnityEngine;

#region Data Structures
/// <summary>
/// Defines a specific atom type and the quantity required for a chemical recipe.
/// </summary>
[System.Serializable]
public struct AtomRequirement
{
    [Tooltip("The type of atom required (e.g., Hydrogen).")]
    public AtomData atomType;

    [Tooltip("The exact number of this atom type needed for a valid bond.")]
    public int requiredCount;
}
#endregion

/// <summary>
/// A ScriptableObject container that defines a valid molecular recipe.
/// This data-driven approach allows for easy expansion of the chemical library.
/// </summary>
[CreateAssetMenu(fileName = "NewMolecule", menuName = "ChemistryLab/Molecule Data")]
public class MoleculeData : ScriptableObject
{
    #region Molecule Metadata
    [Header("Molecule Identity")]
    [Tooltip("The common name of the molecule (e.g., 'Water').")]
    public string moleculeName;

    [Tooltip("The chemical notation (e.g., 'H2O').")]
    public string chemicalFormula;
    #endregion

    #region Recipe & Results
    [Header("Synthesis Recipe")]
    [Tooltip("The list of atoms and quantities required to trigger a successful bond.")]
    public List<AtomRequirement> requiredAtoms;

    [Header("Visual Representation")]
    [Tooltip("The 3D prefab spawned upon successful synthesis, featuring labeled bonds.")]
    public GameObject completedPrefab;
    #endregion
}