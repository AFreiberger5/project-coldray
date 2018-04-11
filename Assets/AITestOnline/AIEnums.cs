/******************************************
*                                         *
*   Script made by Alexander Freiberger   *
*                                         *
*   Edited by:                            *
*                                         *
******************************************/

[System.Flags]
public enum AIState
{
    NONE        = 0,
    ALIVE       = 1,
    IDLE        = 2,
    AWARE       = 4,
    MOVING      = 8,
    ATTACKING   = 16,
    DYING       = 32,
    DEAD        = 64

}