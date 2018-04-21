using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{


    public static Vector3 ScreenToWorldPoint(Vector3 _screenP, float _height, Camera _cam)
    {
        Ray ray = _cam.ScreenPointToRay(_screenP);
        Plane plane = new Plane(Vector3.up, new Vector3(0, _height, 0));
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}
