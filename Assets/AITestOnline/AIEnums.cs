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
    DEAD = 0,
    ALIVE = 1,
    IDLE = 2,
    MOVING = 4,
    ATTACKING = 8,
    DYING = 16,

}