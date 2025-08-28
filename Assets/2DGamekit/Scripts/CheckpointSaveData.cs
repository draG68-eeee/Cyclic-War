using System;
using System.Collections.Generic;

[Serializable]
public class CheckpointSaveData
{
    public int ellaQuestlinePhase = 0;
    public string currentCheckpointID;
    public List<string> visitedCheckpoints = new List<string>();
}
