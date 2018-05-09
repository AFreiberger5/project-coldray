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
    protected abstract void OnNPCSpawn();

    /// <summary>
    /// Method to handle the death of the NPC
    /// </summary>
    protected abstract void KillNPC();

    /// <summary>
    /// To be executed when NPC is dying, handles loot drops and similar.
    /// </summary>
    protected abstract void OnNPCDeath();

    /// <summary>
    /// Handles the Decisionmaking of the NPC based on current State and possible options.
    /// </summary>
    protected abstract void NPCDecision();

    /// <summary>
    /// Called from Player when interacting with the NPC for Combat and Quests
    /// </summary>
    public abstract void OnInteraction();

    /// <summary>
    /// Called from Player when interacting with the NPC for Combat and Quests
    /// </summary>
    /// <param name="_value">Damage/Value received</param>
    /// <param name="_damageType">Type of Attack (Use NONE if not an attack)</param>
    public abstract void OnInteraction(float _value, EDamageType _damageType);

    /// <summary>
    /// Called from Player when interacting with the NPC for Combat and Quests
    /// </summary>
    /// <param name="_obj">(Quest-)object from Player</param>
    public abstract void OnInteraction(object _obj);

    
}
