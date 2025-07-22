using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class CheckpointTeleportMenu : MonoBehaviour
{
    public GameObject menuPanel; // Assign in Inspector: the root panel of the teleport menu
    public Transform checkpointListParent; // Assign in Inspector: parent for checkpoint buttons
    public GameObject checkpointButtonPrefab; // Assign in Inspector: prefab with Button+Text
    public Button closeButton; // Assign in Inspector
    public GameObject player; // Assign in Inspector or auto-find

    private void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMenu);
        menuPanel.SetActive(false);
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        PopulateCheckpointList();
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        // Optionally re-enable player control here
    }

    private void PopulateCheckpointList()
    {
        // Clear old buttons
        foreach (Transform child in checkpointListParent)
            Destroy(child.gameObject);
        // Get visited checkpoints from CheckpointManager
        var visited = CheckpointManager.Instance.visitedCheckpoints;
        var allCheckpoints = FindObjectsOfType<BonfireCheckpoint>();
        foreach (var id in visited)
        {
            var cp = allCheckpoints.FirstOrDefault(c => c.checkpointID == id);
            if (cp != null)
            {
                var btnObj = Instantiate(checkpointButtonPrefab, checkpointListParent);
                btnObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = id;

                btnObj.GetComponent<Button>().onClick.AddListener(() => TeleportToCheckpoint(cp));
            }
        }
    }

    private void TeleportToCheckpoint(BonfireCheckpoint checkpoint)
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (player != null && checkpoint.respawnPoint != null)
        {
            player.transform.position = checkpoint.respawnPoint.position;
            CheckpointManager.Instance.currentCheckpoint = checkpoint;
            CloseMenu();
        }
    }

    // Example: open menu with key (e.g. T)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!menuPanel.activeSelf)
                OpenMenu();
            else
                CloseMenu();
        }
    }
}
