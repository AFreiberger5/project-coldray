/******************************************
*                                         *
*   Script made by Alexander Freiberger   *
*                                         *
*   Edited by:                            *
*                                         *
******************************************/
using System.Collections.Generic;
using UnityEngine.Networking;

public abstract class AIBase : NetworkBehaviour
{
    

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

    /// <summary>
    /// Calculates Recieved Damage based on NPC defense
    /// </summary>
    /// <param name="_damage">The unmitigated Damage</param>
    /// <param name="_damageType">Type(s) of Damage recieved</param>
    /// <param name="_defenseList">Dictionary with the defensive Values against each Type</param>
    /// <returns></returns>
    public virtual float DamageCalculation(float _damage, EDamageType _damageType, Dictionary<EDamageType, float> _defenseList)
    {
        if(_defenseList != null && _defenseList.Count > 0)
        foreach (KeyValuePair<EDamageType, float> Type in _defenseList)
        {
            if (_damageType.HasFlag(Type.Key))
                _damage -= Type.Value;
        }

        //If Defense are higher than damage, no damage is dealt, negative damage does not heal the NPC, but it might Tickle him to death
        return (_damage > 0) ? _damage : 0;
    }


}
