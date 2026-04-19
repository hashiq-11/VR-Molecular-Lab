using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A centralized data container that holds every valid molecular recipe for the lab.
/// This registry allows the BondManager to remain generic and data-driven.
/// </summary>
[CreateAssetMenu(fileName = "MoleculeDatabase", menuName = "ChemistryLab/Molecule Database")]
public class MoleculeDatabase : ScriptableObject
{
    #region Registry
    [Header("Molecular Recipes")]
    [Tooltip("The master list of all valid MoleculeData assets recognized by the system.")]
    public List<MoleculeData> validMolecules;
    #endregion
}