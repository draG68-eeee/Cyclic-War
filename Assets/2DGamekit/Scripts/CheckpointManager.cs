using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    public BonfireCheckpoint currentCheckpoint;
    public List<string> visitedCheckpoints = new List<string>();
    private const string SaveKey = "BonfireCheckpoint_Save";
    private const string VisitedKey = "BonfireCheckpoint_Visited";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            LoadCheckpoint();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadCheckpoint();
    }

    

    public void ActivateCheckpoint(BonfireCheckpoint checkpoint)
    {
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
        if (currentCheckpoint != null)
        {
            PlayerPrefs.SetString(SaveKey, currentCheckpoint.checkpointID);
        }
        PlayerPrefs.SetString(VisitedKey, string.Join(",", visitedCheckpoints));
        PlayerPrefs.Save();
    }

    public void LoadCheckpoint()
    {
        visitedCheckpoints.Clear();
        string visited = PlayerPrefs.GetString(VisitedKey, "");
        if (!string.IsNullOrEmpty(visited))
        {
            visitedCheckpoints.AddRange(visited.Split(','));
        }
        string checkpointID = PlayerPrefs.GetString(SaveKey, "");
        if (!string.IsNullOrEmpty(checkpointID))
        {
            BonfireCheckpoint[] all = FindObjectsOfType<BonfireCheckpoint>();
            foreach (var cp in all)
            {
                if (cp.checkpointID == checkpointID)
                {
                    currentCheckpoint = cp;
                    break;
                }
            }
        }
    }
}
