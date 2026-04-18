using UnityEngine;
using System.Collections.Generic;

public class BondManager : MonoBehaviour
{
    public static BondManager Instance { get; private set; }

    [Header("Data")]
    public MoleculeDatabase database; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void TryBond(AtomController atom1, AtomController atom2)
    {
        atom1.isBonded = true;
        atom2.isBonded = true;

        // 1. Tally up the atoms involved in this collision
        Dictionary<AtomData, int> currentAtomCounts = new Dictionary<AtomData, int>();
        currentAtomCounts[atom1.atomData] = 1;

        if (currentAtomCounts.ContainsKey(atom2.atomData))
            currentAtomCounts[atom2.atomData]++;
        else
            currentAtomCounts[atom2.atomData] = 1;

        // 2. Check our dictionary against the database recipes
        MoleculeData matchedMolecule = GetMatchingMolecule(currentAtomCounts);

        if (matchedMolecule != null)
        {
            Debug.Log($"Success! Formed: {matchedMolecule.moleculeName} ({matchedMolecule.chemicalFormula})");
            SpawnCompletedMolecule(matchedMolecule, atom1, atom2);
        }
        else
        {
            Debug.Log("Invalid combination. Bouncing off.");
            atom1.isBonded = false;
            atom2.isBonded = false;
        }
    }

    private MoleculeData GetMatchingMolecule(Dictionary<AtomData, int> currentCounts)
    {
        foreach (MoleculeData recipe in database.validMolecules)
        {
            if (IsExactMatch(currentCounts, recipe))
            {
                return recipe;
            }
        }
        return null; // No match found
    }

    private bool IsExactMatch(Dictionary<AtomData, int> currentCounts, MoleculeData recipe)
    {
        // First, check if the total number of UNIQUE atom types matches
        if (currentCounts.Count != recipe.requiredAtoms.Count) return false;

        // Next, check if the exact quantities match the recipe
        foreach (AtomRequirement req in recipe.requiredAtoms)
        {
            if (!currentCounts.ContainsKey(req.atomType)) return false;
            if (currentCounts[req.atomType] != req.requiredCount) return false;
        }

        return true;
    }

    private void SpawnCompletedMolecule(MoleculeData molecule, AtomController a, AtomController b)
    {
        // Find the midpoint between the two atoms to spawn the new molecule
        Vector3 spawnPosition = (a.transform.position + b.transform.position) / 2f;

        if (molecule.completedPrefab != null)
        {
            Instantiate(molecule.completedPrefab, spawnPosition, Quaternion.identity);
        }

        // Destroy the loose atoms
        Destroy(a.gameObject);
        Destroy(b.gameObject);
    }
    /// <summary>
    /// Evaluates a list of atoms to see if they form a valid molecule.
    /// </summary>
    public bool TryBondFlask(List<AtomController> atomsInFlask, Vector3 spawnPosition)
    {
        // 1. Tally up all the atoms in the flask
        Dictionary<AtomData, int> currentCounts = new Dictionary<AtomData, int>();

        foreach (AtomController atom in atomsInFlask)
        {
            if (currentCounts.ContainsKey(atom.atomData))
                currentCounts[atom.atomData]++;
            else
                currentCounts[atom.atomData] = 1;
        }

        // 2. Check the dictionary against the database recipes
        MoleculeData matchedMolecule = GetMatchingMolecule(currentCounts);

        if (matchedMolecule != null)
        {
            Debug.Log($"[Flask] Success! Formed: {matchedMolecule.moleculeName} ({matchedMolecule.chemicalFormula})");

            // 3. Spawn the completed molecule
            if (matchedMolecule.completedPrefab != null)
            {
                Instantiate(matchedMolecule.completedPrefab, spawnPosition, Quaternion.identity);
            }

            // 4. Destroy the loose atoms
            foreach (AtomController atom in atomsInFlask)
            {
                if (atom != null) Destroy(atom.gameObject);
            }

            return true;
        }

        // Optional: Play a "failed buzz" audio clip here later
        Debug.Log("[Flask] Reaction failed. Invalid combination.");
        return false;
    }
}