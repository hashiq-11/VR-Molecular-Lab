using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// Manages the World-Space molecule discovery log. 
/// Handles duplicate prevention, dynamic UI list population, and satisfying visual feedback.
/// </summary>
public class LibraryManager : MonoBehaviour
{
    #region Singleton
    public static LibraryManager Instance { get; private set; }
    #endregion

    #region UI References
    [Header("UI References")]
    [Tooltip("The main panel container used for entrance and feedback animations.")]
    public Transform panelTransform;

    [Tooltip("The parent Layout Group where new discovery items are instantiated.")]
    public Transform listContainer;

    [Tooltip("A hidden UI prefab used as a template for new discovery entries.")]
    public GameObject itemTemplate;

    [Tooltip("The header text tracking progress toward the minimum goal of 7 molecules.")]
    public TextMeshProUGUI headerText;
    #endregion

    #region Data & State
    // HashSet provides O(1) lookup time for efficient duplicate checking.
    private HashSet<string> discoveredMolecules = new HashSet<string>();

    // A Queue handles FIFO logic for cycling the UI list items.
    private Queue<GameObject> activeUIItems = new Queue<GameObject>();

    [SerializeField] private int maxVisibleItems = 4;
    #endregion

    #region Lifecycle
    private void Awake()
    {
        // Enforce the Singleton pattern for global access
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        // Clean up the template state
        if (itemTemplate != null) itemTemplate.SetActive(false);

        UpdateHeader();

        // Entrance 'Juice': Smooth scaling transition for VR immersion.
        if (panelTransform != null)
        {
            panelTransform.localScale = Vector3.zero;
            panelTransform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBack);
        }
    }
    #endregion

    #region Public API
    /// <summary>
    /// Logs a new molecule, updates progress, and manages UI list cycling.
    /// </summary>
    public void AddDiscovery(string moleculeName, string formula)
    {
        string uniqueKey = $"{moleculeName}_{formula}";

        // Guard clause: Only process new discoveries
        if (!discoveredMolecules.Contains(uniqueKey))
        {
            discoveredMolecules.Add(uniqueKey);
            UpdateHeader();

            // --- 1. UI RECYCLING ---
            // Maintain a clean XR layout and optimize draw calls by removing oldest entries.
            if (activeUIItems.Count >= maxVisibleItems)
            {
                GameObject oldestItem = activeUIItems.Dequeue();
                Destroy(oldestItem);
            }

            // --- 2. DYNAMIC INSTANTIATION ---
            GameObject newItem = Instantiate(itemTemplate, listContainer);
            newItem.SetActive(true);

            TextMeshProUGUI textComp = newItem.GetComponent<TextMeshProUGUI>();
            if (textComp != null)
            {
                textComp.text = $"• {moleculeName} ({formula})";
            }

            activeUIItems.Enqueue(newItem);

            // --- 3. POLISH & JUICE ---
            // Visual affordance: Pop and punch effects to draw user attention to new data.
            newItem.transform.localScale = Vector3.zero;
            newItem.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);

            if (panelTransform != null)
            {
                panelTransform.DOPunchScale(new Vector3(0.05f, 0.05f, 0f), 0.3f, 2, 0.5f);
            }
        }
    }
    #endregion

    #region Internal Logic
    private void UpdateHeader()
    {
        if (headerText != null)
        {
            // Tracks assessment criteria for the 7-molecule minimum discovery goal.
            headerText.text = $"DISCOVERED: {discoveredMolecules.Count} / 7";
        }
    }
    #endregion
}