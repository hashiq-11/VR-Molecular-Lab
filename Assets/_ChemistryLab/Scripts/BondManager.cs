using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The central brain for chemical validation. Uses a data-driven approach 
/// to match physical atom collections against a Molecule Database.
/// </summary>
public class BondManager : MonoBehaviour
{
    #region Singleton
    public static BondManager Instance { get; private set; }

    private void Awake()
    {
        // Strict Singleton enforcement to prevent logic conflicts in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    #region Configuration
    [Header("Data Architecture")]
    [Tooltip("Master registry of all valid chemical combinations.")]
    public MoleculeDatabase database;
    #endregion

    #region Primary Interaction Logic
    /// <summary>
    /// Processes a collision-based bond attempt between two discrete atoms.
    /// </summary>
    public void TryBond(AtomController atom1, AtomController atom2)
    {
        // 1. STATE LOCK: Prevent race conditions where an atom bonds twice in the same frame
        atom1.isBonded = true;
        atom2.isBonded = true;

        // 2. DATA AGGREGATION: Using a Dictionary makes the recipe check order-independent (O+H == H+O)
        Dictionary<AtomData, int> currentAtomCounts = new Dictionary<AtomData, int>();
        currentAtomCounts[atom1.atomData] = 1;

        if (currentAtomCounts.ContainsKey(atom2.atomData))
            currentAtomCounts[atom2.atomData]++;
        else
            currentAtomCounts[atom2.atomData] = 1;

        // 3. RECIPE MATCHING
        MoleculeData matchedMolecule = GetMatchingMolecule(currentAtomCounts);

        if (matchedMolecule != null)
        {
            Debug.Log($"[Bonding] Success: {matchedMolecule.moleculeName}");
            SpawnCompletedMolecule(matchedMolecule, atom1, atom2);
        }
        else
        {
            // 4. FAILSTATE: Release the lock so atoms can react with other elements
            Debug.Log("[Bonding] Invalid combination.");
            atom1.isBonded = false;
            atom2.isBonded = false;
        }
    }

    /// <summary>
    /// Evaluates a collection of atoms (e.g., from a flask) against the database.
    /// </summary>
    public bool TryBondFlask(List<AtomController> atomsInFlask, Vector3 spawnPosition)
    {
        // 1. TALLY: Convert the list of physical objects into a mathematical count
        Dictionary<AtomData, int> currentCounts = new Dictionary<AtomData, int>();

        foreach (AtomController atom in atomsInFlask)
        {
            if (currentCounts.ContainsKey(atom.atomData))
                currentCounts[atom.atomData]++;
            else
                currentCounts[atom.atomData] = 1;
        }

        // 2. EVALUATION
        MoleculeData matchedMolecule = GetMatchingMolecule(currentCounts);

        if (matchedMolecule != null)
        {
            FinalizeReaction(matchedMolecule, atomsInFlask, spawnPosition);
            return true;
        }

        return false;
    }
    #endregion

    #region Helper & Math Logic
    private MoleculeData GetMatchingMolecule(Dictionary<AtomData, int> currentCounts)
    {
        // O(n) search through the database. Efficient enough for small-to-medium lab sims.
        foreach (MoleculeData recipe in database.validMolecules)
        {
            if (IsExactMatch(currentCounts, recipe)) return recipe;
        }
        return null;
    }

    private bool IsExactMatch(Dictionary<AtomData, int> currentCounts, MoleculeData recipe)
    {
        // OPTIMIZATION: Early exit if unique atom counts don't align
        if (currentCounts.Count != recipe.requiredAtoms.Count) return false;

        // Deep verification of atom quantities
        foreach (AtomRequirement req in recipe.requiredAtoms)
        {
            if (!currentCounts.ContainsKey(req.atomType)) return false;
            if (currentCounts[req.atomType] != req.requiredCount) return false;
        }

        return true;
    }
    #endregion

    #region Cleanup & Spawning
    private void SpawnCompletedMolecule(MoleculeData molecule, AtomController a, AtomController b)
    {
        Vector3 spawnPosition = (a.transform.position + b.transform.position) / 2f;

        if (molecule.completedPrefab != null)
            Instantiate(molecule.completedPrefab, spawnPosition, Quaternion.identity);

        // Notify systems
        TriggerExternalManagers(molecule);

        // Cleanup raw materials
        Destroy(a.gameObject);
        Destroy(b.gameObject);
    }

    private void FinalizeReaction(MoleculeData molecule, List<AtomController> ingredients, Vector3 spawnPos)
    {
        if (molecule.completedPrefab != null)
            Instantiate(molecule.completedPrefab, spawnPos, Quaternion.identity);

        TriggerExternalManagers(molecule);

        // Mass-destruction of consumed ingredients
        foreach (AtomController atom in ingredients)
        {
            if (atom != null) Destroy(atom.gameObject);
        }
    }

    private void TriggerExternalManagers(MoleculeData molecule)
    {
        // UI Notification
        if (LibraryManager.Instance != null)
            LibraryManager.Instance.AddDiscovery(molecule.moleculeName, molecule.chemicalFormula);

        // SFX Trigger
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySuccess();
        else
            Debug.LogWarning("[Architecture] AudioManager reference missing.");
    }
    #endregion
}