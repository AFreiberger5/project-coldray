/******************************************
*                                         *
*   Script made by Alexander Bloomenkamp  *
*                                         *
*   Edited by:                            *
*                                         *
******************************************/
using UnityEngine;

public class Utils {

	static int MAXHEIGHT = 16;
	static float SMOOTH = 0.001f;
	static int OCTAVES = 1;
	static float PERSISTENCE = 0.5f;
    

    public static int GenerateCliffHeight(float _x, float _z)
    {
        float height = Map(0, MAXHEIGHT - 24, 0, 1, FBM(_x * SMOOTH * 4, _z * SMOOTH * 4, OCTAVES + 3, PERSISTENCE));
        return (int)height;
    }
    public static int GenerateStoneHeight(float _x, float _z)
	{
		float height = Map(0,MAXHEIGHT, 0, 1, FBM(_x*SMOOTH*4,_z*SMOOTH*4,OCTAVES+2,PERSISTENCE));
		return (int) height;
	}

	public static int GenerateHeight(float _x, float _z)
	{
		float height = Map(0,MAXHEIGHT, 0, 1, FBM(_x*SMOOTH,_z*SMOOTH,OCTAVES,PERSISTENCE));
		return (int) height;
	}

    public static float FBM3D(float _x, float _y, float _z, float _sm, int _oct)
    {
        float XY = FBM(_x*_sm,_y*_sm,_oct,0.5f);
        float YZ = FBM(_y*_sm,_z*_sm,_oct,0.5f);
        float XZ = FBM(_x*_sm,_z*_sm,_oct,0.5f);

        float YX = FBM(_y*_sm,_x*_sm,_oct,0.5f);
        float ZY = FBM(_z*_sm,_y*_sm,_oct,0.5f);
        float ZX = FBM(_z*_sm,_x*_sm,_oct,0.5f);

        return (XY+YZ+XZ+YX+ZY+ZX)/6.0f;
    }

	static float Map(float _newmin, float _newmax, float _origmin, float _origmax, float _value)
    {
        return Mathf.Lerp (_newmin, _newmax, Mathf.InverseLerp (_origmin, _origmax, _value));
    }

    static float FBM(float _x, float _z, int _oct, float _pers)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        float offset = 32000f;
        for(int i = 0; i < _oct ; i++) 
        {
                total += Mathf.PerlinNoise((_x+ offset) * frequency, (_z+offset) * frequency) * amplitude;

                maxValue += amplitude;
                amplitude *= _pers;
                frequency *= 2;
        }

        return total/maxValue;
    }
}
