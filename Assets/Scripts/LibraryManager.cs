using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public class LibraryManager : MonoBehaviour
{
    public static LibraryManager Instance { get; private set; }

    [Header("UI References")]
    public Transform panelTransform;
    public Transform listContainer;
    public GameObject itemTemplate;

    [Tooltip("Drag the Header text here to update the score")]
    public TextMeshProUGUI headerText;

    // Track EVERYTHING they found to prevent duplicates
    private HashSet<string> discoveredMolecules = new HashSet<string>();

    // Track only the VISIBLE UI items
    private Queue<GameObject> activeUIItems = new Queue<GameObject>();

    private int maxVisibleItems = 4; // Max items before we delete the oldest

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        if (itemTemplate != null) itemTemplate.SetActive(false);
        UpdateHeader();

        if (panelTransform != null)
        {
            panelTransform.localScale = Vector3.zero;
            panelTransform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBack);
        }
    }

    public void AddDiscovery(string moleculeName, string formula)
    {
        string uniqueKey = $"{moleculeName}_{formula}";

        if (!discoveredMolecules.Contains(uniqueKey))
        {
            discoveredMolecules.Add(uniqueKey);
            UpdateHeader();

            // 1. If we have too many items on the board, destroy the oldest one
            if (activeUIItems.Count >= maxVisibleItems)
            {
                GameObject oldestItem = activeUIItems.Dequeue();
                Destroy(oldestItem);
            }

            // 2. Clone the template for the NEW discovery
            GameObject newItem = Instantiate(itemTemplate, listContainer);
            newItem.SetActive(true);

            TextMeshProUGUI textComp = newItem.GetComponent<TextMeshProUGUI>();
            if (textComp != null)
            {
                textComp.text = $"• {moleculeName} ({formula})";
            }

            // Add it to our visible queue
            activeUIItems.Enqueue(newItem);

            // 3. DOTween Polish
            newItem.transform.localScale = Vector3.zero;
            newItem.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);

            if (panelTransform != null)
            {
                panelTransform.DOPunchScale(new Vector3(0.05f, 0.05f, 0f), 0.3f, 2, 0.5f);
            }
        }
    }

    private void UpdateHeader()
    {
        if (headerText != null)
        {
            // Updates the title to show progress toward the minimum 7 requirement
            headerText.text = $"DISCOVERED: {discoveredMolecules.Count} / 7";
        }
    }

    // DEBUG TOOLS
    [ContextMenu("Debug: Discover Water")]
    public void DebugDiscoverWater() { AddDiscovery("Water", "H2O"); }
    [ContextMenu("Debug: Discover Ammonia")]
    public void DebugDiscoverAmmonia() { AddDiscovery("Ammonia", "NH3"); }
    [ContextMenu("Debug: Discover Methane")]
    public void DebugDiscoverMethane() { AddDiscovery("Methane", "CH4"); }
    [ContextMenu("Debug: Discover Oxygen")]
    public void DebugDiscoverOxygen() { AddDiscovery("Oxygen Gas", "O2"); }
    [ContextMenu("Debug: Discover Hydrogen")]
    public void DebugDiscoverHydrogen() { AddDiscovery("Hydrogen Gas", "H2"); }
    [ContextMenu("Debug: Discover Nitrogen")]
    public void DebugDiscoverNitrogen() { AddDiscovery("Nitrogen Gas", "N2"); }
    [ContextMenu("Debug: Discover Carbon Dioxide")]
    public void DebugDiscoverCarbonDioxide() { AddDiscovery("Carbon Dioxide", "CO2"); }
}