/**************************************************
*  Credits: Created by Alexander Freiberger		  *
*                                                 *
*  Additional Edits by:                           *
*                                                 *
*                                                 *
*                                                 *
***************************************************/

using System;

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
