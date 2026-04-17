using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoleculeDatabase", menuName = "ChemistryLab/Molecule Database")]
public class MoleculeDatabase : ScriptableObject
{
    public List<MoleculeData> validMolecules;
}