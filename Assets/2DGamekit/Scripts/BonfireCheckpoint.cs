using UnityEngine;
using System.Collections.Generic;

public class BonfireCheckpoint : MonoBehaviour
{
    public string checkpointID;
    public Transform respawnPoint; // The location to respawn the player

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager.Instance.ActivateCheckpoint(this);
        }
    }
}

// public class CheckpointManager : MonoBehaviour
// {
//     public static CheckpointManager Instance;

//     public BonfireCheckpoint currentCheckpoint;
//     public List<string> visitedCheckpoints = new List<string>();
//     private const string SaveKey = "BonfireCheckpoint_Save";
//     private const string VisitedKey = "BonfireCheckpoint_Visited";

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//             LoadCheckpoint();
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     public void ActivateCheckpoint(BonfireCheckpoint checkpoint)
//     {
//         currentCheckpoint = checkpoint;
//         if (!visitedCheckpoints.Contains(checkpoint.checkpointID))
//         {
//             visitedCheckpoints.Add(checkpoint.checkpointID);
//         }
//         SaveCheckpoint();
//     }

//     public void RespawnPlayer(GameObject player)
//     {
//         if (currentCheckpoint != null && currentCheckpoint.respawnPoint != null)
//         {
//             player.transform.position = currentCheckpoint.respawnPoint.position;
//         }
//     }

//     public void SaveCheckpoint()
//     {
//         if (currentCheckpoint != null)
//         {
//             PlayerPrefs.SetString(SaveKey, currentCheckpoint.checkpointID);
//         }
//         PlayerPrefs.SetString(VisitedKey, string.Join(",", visitedCheckpoints));
//         PlayerPrefs.Save();
//     }

//     public void LoadCheckpoint()
//     {
//         visitedCheckpoints.Clear();
//         string visited = PlayerPrefs.GetString(VisitedKey, "");
//         if (!string.IsNullOrEmpty(visited))
//         {
//             visitedCheckpoints.AddRange(visited.Split(','));
//         }
//         string checkpointID = PlayerPrefs.GetString(SaveKey, "");
//         if (!string.IsNullOrEmpty(checkpointID))
//         {
//             BonfireCheckpoint[] all = FindObjectsOfType<BonfireCheckpoint>();
//             foreach (var cp in all)
//             {
//                 if (cp.checkpointID == checkpointID)
//                 {
//                     currentCheckpoint = cp;
//                     break;
//                 }
//             }
//         }
//     }
// }
