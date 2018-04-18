/******************************************
*                                         *
*   Script made by Alexander Freiberger   *
*                                         *
*   Edited by:                            *
*                                         *
******************************************/
using UnityEngine.Networking;

public abstract class AIBase : NetworkBehaviour 
{
    internal const int MAX_PLAYERCOUNT = 4;

    /// <summary>
    /// To be executed when NPC is spawned.
    /// </summary>
    public abstract void OnNPCSpawn();

    /// <summary>
    /// Method to handle the death of the NPC
    /// </summary>
    public abstract void KillNPC();

    /// <summary>
    /// To be executed when NPC is dying, handles loot drops and similar.
    /// </summary>
    protected abstract void OnNPCDeath();

    /// <summary>
    /// Handles the Decisionmaking of the NPC based on current State and possible options.
    /// </summary>
    public abstract void NPCDecision();
}
