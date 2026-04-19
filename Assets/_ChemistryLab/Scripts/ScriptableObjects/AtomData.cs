using UnityEngine;

/// <summary>
/// A data-driven container (ScriptableObject) defining the chemical and visual properties of an element.
/// Implementation Note: Acts as a 'Flyweight' to minimize memory overhead by sharing 
/// data across all instances of a specific atom type.
/// </summary>
[CreateAssetMenu(fileName = "NewAtom", menuName = "ChemistryLab/Atom Data")]
public class AtomData : ScriptableObject
{
    #region Element Metadata
    [Header("Element Identity")]
    [Tooltip("Full name of the element. Used for UI notifications and Discovery Log entries.")]
    public string elementName;

    [Tooltip("The periodic table symbol (e.g., H, O, C). Essential for dynamic formula generation.")]
    public string symbol;
    #endregion

    #region Chemistry Constraints
    [Header("Valence Properties")]
    [Tooltip("The maximum number of covalent bonds this atom can form. Limits bonding logic in BondManager.")]
    public int maxBonds;
    #endregion

    #region Visual Configuration
    [Header("Appearance")]
    [Tooltip("The standardized CPK color for this element, applied to the atom mesh at runtime.")]
    public Color atomColor = Color.white;
    #endregion
}