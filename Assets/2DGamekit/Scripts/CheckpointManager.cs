using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    public int ellaQuestlinePhase = 0;
    public static CheckpointManager Instance;

    public BonfireCheckpoint currentCheckpoint;
    public List<string> visitedCheckpoints = new List<string>();
    private int SelectedSlot => PlayerPrefs.GetInt("SelectedSaveSlot", 0);
    private string GetSaveFilePath(int slot)
    {
        return System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, $"save_slot_{slot}.json");
    }

    private void Awake()
    {
        Debug.Log("[CheckpointManager] Awake called");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("[CheckpointManager] Set as singleton instance and subscribed to sceneLoaded");
            LoadCheckpoint();
        }
        else
        {
            Debug.LogWarning("[CheckpointManager] Duplicate instance detected, destroying this gameObject");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("[CheckpointManager] OnDestroy called");
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Debug.Log("[CheckpointManager] Unsubscribed from sceneLoaded");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[CheckpointManager] Scene loaded: {scene.name}, calling LoadCheckpoint()");
        LoadCheckpoint();
    }

    

    public void ActivateCheckpoint(BonfireCheckpoint checkpoint)
    {
        Debug.Log($"[CheckpointManager] ActivateCheckpoint called for checkpoint: {checkpoint.checkpointID}");
        currentCheckpoint = checkpoint;
        if (!visitedCheckpoints.Contains(checkpoint.checkpointID))
        {
            visitedCheckpoints.Add(checkpoint.checkpointID);
        }
        SaveCheckpoint();
    }

    public void RespawnPlayer(GameObject player)
    {
        if (currentCheckpoint != null && currentCheckpoint.respawnPoint != null)
        {
            player.transform.position = currentCheckpoint.respawnPoint.position;
        }
    }

    public void SaveCheckpoint()
    {
        Debug.Log($"[SaveCheckpoint] Called for slot {SelectedSlot}");
        CheckpointSaveData data = new CheckpointSaveData();
        data.currentCheckpointID = currentCheckpoint != null ? currentCheckpoint.checkpointID : "";
        data.visitedCheckpoints = new List<string>(visitedCheckpoints);
        data.ellaQuestlinePhase = ellaQuestlinePhase;
        SaveToFile(SelectedSlot, data);
    }

    public void LoadCheckpoint()
    {
        Debug.Log($"[LoadCheckpoint] Called for slot {SelectedSlot}");
        visitedCheckpoints.Clear();
        CheckpointSaveData data = LoadFromFile(SelectedSlot);
        if (data != null)
        {
            Debug.Log("[LoadCheckpoint] Save file loaded successfully.");
            visitedCheckpoints = new List<string>(data.visitedCheckpoints);
            ellaQuestlinePhase = data.ellaQuestlinePhase;
            if (!string.IsNullOrEmpty(data.currentCheckpointID))
            {
                BonfireCheckpoint[] all = FindObjectsOfType<BonfireCheckpoint>();
                foreach (var cp in all)
                {
                    if (cp.checkpointID == data.currentCheckpointID)
                    {
                        currentCheckpoint = cp;
                        break;
                    }
                }
            }
        }
    }

    private void SaveToFile(int slot, CheckpointSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
#if UNITY_WEBGL
        Debug.Log($"[SaveToFile] (WebGL) Saving slot {slot} to PlayerPrefs");
        PlayerPrefs.SetString($"save_slot_{slot}_json", json);
        PlayerPrefs.Save();
#else
        try
        {
            string path = GetSaveFilePath(slot);
            Debug.Log($"[SaveToFile] Saving slot {slot} to file: {path}");
            System.IO.File.WriteAllText(path, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save checkpoint: " + e);
        }
#endif
    }

    private CheckpointSaveData LoadFromFile(int slot)
    {
        string json = null;
#if UNITY_WEBGL
        Debug.Log($"[LoadFromFile] (WebGL) Loading slot {slot} from PlayerPrefs");
        json = PlayerPrefs.GetString($"save_slot_{slot}_json", null);
#else
        try
        {
            string path = GetSaveFilePath(slot);
            Debug.Log($"[LoadFromFile] Loading slot {slot} from file: {path}");
            if (System.IO.File.Exists(path))
            {
                json = System.IO.File.ReadAllText(path);
            }
            else
            {
                Debug.LogWarning($"[LoadFromFile] No save file found at {path}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load checkpoint: " + e);
        }
#endif
        if (!string.IsNullOrEmpty(json))
            return JsonUtility.FromJson<CheckpointSaveData>(json);
        Debug.LogWarning($"[LoadFromFile] No valid JSON found for slot {slot}");
        return null;
    }

    public void SetEllaQuestlinePhase(int phase)
    {
        ellaQuestlinePhase = phase;
        SaveCheckpoint();
    }
}


