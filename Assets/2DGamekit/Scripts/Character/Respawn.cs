using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CheckpointManager checkpointManager;
    void Start()
    {
        CheckpointManager.Instance.RespawnPlayer(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
