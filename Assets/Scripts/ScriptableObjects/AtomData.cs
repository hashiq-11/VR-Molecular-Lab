using UnityEngine;

[CreateAssetMenu(fileName = "NewAtom", menuName = "ChemistryLab/Atom Data")]
public class AtomData : ScriptableObject
{
    [Header("Element Properties")]
    public string elementName; // e.g., "Oxygen"
    public string symbol;      // e.g., "O"
    public int maxBonds;       // e.g., 2

    [Header("Visuals")]
    public Color atomColor = Color.white; // Makes it easy to color the basic spheres
}