using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleScreenManager : MonoBehaviour
{
    public Button[] saveSlotButtons; // Assign 3 buttons in Inspector
    public Button newGameButton;
    public GameObject continuePanel;
    public Transform checkpointListParent;
    public GameObject checkpointListItemPrefab;

    private int selectedSlot = -1;
    private const int maxSlots = 3;


    void Start()
    {
        for (int i = 0; i < saveSlotButtons.Length; i++)
        {
            int slot = i;
            saveSlotButtons[i].onClick.AddListener(() => OnSaveSlotClicked(slot));
        }
        newGameButton.onClick.AddListener(OnNewGameClicked);
        continuePanel.SetActive(false);
    }

    void OnSaveSlotClicked(int slot)
    {
        selectedSlot = slot;
        continuePanel.SetActive(true);
        PopulateCheckpointList();
    }

    void OnNewGameClicked()
    {
        if (selectedSlot == -1) return;
        // Clear the selected save slot
        PlayerPrefs.DeleteKey(GetSaveKey(selectedSlot, "BonfireCheckpoint_Save"));
        PlayerPrefs.DeleteKey(GetSaveKey(selectedSlot, "BonfireCheckpoint_Visited"));
        PlayerPrefs.SetInt("SelectedSaveSlot", selectedSlot);
        PlayerPrefs.Save();
        SceneManager.LoadScene("World of Eternal Cycle"); // replace with your main scene name
    }

    void PopulateCheckpointList()
    {
        foreach (Transform child in checkpointListParent)
            Destroy(child.gameObject);

        string visited = PlayerPrefs.GetString(GetSaveKey(selectedSlot, "BonfireCheckpoint_Visited"), "");
        if (!string.IsNullOrEmpty(visited))
        {
            foreach (var cp in visited.Split(','))
            {
                var item = Instantiate(checkpointListItemPrefab, checkpointListParent);
                item.GetComponentInChildren<Text>().text = cp;
            }
        }
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("SelectedSaveSlot", selectedSlot);
        PlayerPrefs.Save();
        SceneManager.LoadScene("World of Eternal Cycle"); // replace with your main scene name
    }

    string GetSaveKey(int slot, string key) => $"SaveSlot_{slot}_{key}";
}
