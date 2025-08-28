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
        Debug.Log($"[OnNewGameClicked] Slot: {selectedSlot}");
        if (selectedSlot == -1) return;
        PlayerPrefs.SetInt("SelectedSaveSlot", selectedSlot);
        PlayerPrefs.Save();
    #if UNITY_WEBGL
        PlayerPrefs.DeleteKey($"save_slot_{selectedSlot}_json");
        PlayerPrefs.Save();
    #else
        string path = System.IO.Path.Combine(Application.persistentDataPath, $"save_slot_{selectedSlot}.json");
        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);
    #endif
        SceneManager.LoadScene("World of Eternal Cycle");
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
        Debug.Log($"[ContinueGame] Slot: {selectedSlot}");
        PlayerPrefs.SetInt("SelectedSaveSlot", selectedSlot);
        PlayerPrefs.Save();
#if UNITY_WEBGL
        bool hasSave = PlayerPrefs.HasKey($"save_slot_{selectedSlot}_json");
        Debug.Log($"[ContinueGame] (WebGL) Save exists: {hasSave}");
        if (hasSave)
            SceneManager.LoadScene("World of Eternal Cycle");
        else
            Debug.LogWarning("No save found in PlayerPrefs for this slot!");
#else
        string path = System.IO.Path.Combine(Application.persistentDataPath, $"save_slot_{selectedSlot}.json");
        Debug.Log($"[ContinueGame] Checking for save file at: {path}");
        Debug.Log($"[ContinueGame] File exists: {System.IO.File.Exists(path)}");
        if (System.IO.File.Exists(path))
            SceneManager.LoadScene("World of Eternal Cycle");
        else
            Debug.LogWarning("No save file exists for this slot!");
#endif
    }

    string GetSaveKey(int slot, string key) => $"SaveSlot_{slot}_{key}";
}
