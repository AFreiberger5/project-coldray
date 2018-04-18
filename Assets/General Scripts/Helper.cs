/**************************************************
*  Credits: Created by Alexander Freiberger		  *
*                                                 *
*  Additional Edits by:                           *
*                                                 *
*                                                 *
*                                                 *
***************************************************/

using System;
using UnityEngine;

#region Extensions
// Since unity/mono does not support .net 4 or higher I had to write this
// to avoid having to manually check Flags everytime
public static class EnumExtensions
{
    /// <summary>
    /// Checks if the Enumvariable has the flag _value set or not
    /// </summary>
    /// <param name="_variable">The to be tested Enum Variable</param>
    /// <param name="_value">The Flag that is looked for</param>
    /// <returns>Returns true if the variable contains the Flag</returns>
    public static bool HasFlag(this Enum _variable, Enum _value)
    {
        //return and throw exception if the flag cannot be in the tested enum
        if (_variable.GetType() != _value.GetType())
        {
            throw new ArgumentException("The tested Flag does not exis in this Enum");
        }
        //Convert to ulongs to be able to be used with most of the possible enumflag declarations
        ulong FlagNumber = Convert.ToUInt64(_value);
        ulong FlagNumber2 = Convert.ToUInt64(_variable);

        return (FlagNumber2 & FlagNumber) == FlagNumber;
    }

}
#endregion
#region Enums
[Flags]
public enum AIState
{
    NONE = 0,
    ALIVE = 1,
    IDLE = 2,
    AWARE = 4,
    MOVING = 8,
    ATTACKING = 16,
    DYING = 32,
    DEAD = 64

}
public enum DropTable
{
    NONE = 0,
    BANSHEE = 1,
}
#endregion
#region Functions
public class Helper : MonoBehaviour
{

    public void SpawnLoot(DropTable _dropTable, Vector3 _position)
    {

    }

}
#endregion
