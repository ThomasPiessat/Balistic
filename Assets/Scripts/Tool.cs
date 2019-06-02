using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public static class Tool
{
    private const float m_gravity = 9.81f;
    //we give target point (rayCast)
    //=> found the velocity to reach the target
    public static Vector3 CalculateVelocity(Vector3 _target, Vector3 _caster, float _time)
    {
        /***create 2D repere***/
        //init Direction to shoot
        //take care worldAxis
        Vector3 shootDirection = _target - _caster;
        float Py = shootDirection.y;
        shootDirection.y = 0;

        float Px = shootDirection.magnitude;

        float Vx = Px / _time;
        float Vy = Py / _time + 0.5f * m_gravity * _time;

        Vector3 result = shootDirection.normalized;
        result *= Vx;
        result.y = Vy;

        return result;
    }
    /*velocity
     
    V(t) = Vo+ A * t

     */

    public static List<float> GetAngleRad(Vector3 _casterPosition, Vector3 _targetPosition, float _magn)
    {
        /***create 2D repere***/
        //init Direction to shoot
        Vector3 shootDirection = _targetPosition - _casterPosition;
        shootDirection.y = 0;
        shootDirection.Normalize();

        Vector2 beginPosition = new Vector2(shootDirection.x * _casterPosition.x + shootDirection.z * _casterPosition.z, _casterPosition.y);
        Vector2 targetPosition = new Vector2(shootDirection.x * _targetPosition.x + shootDirection.z * _targetPosition.z, _targetPosition.y);

        return GetAngleRad(beginPosition, targetPosition, _magn);
    }

    public static List<float> GetAngle(Vector2 _targetPosition, float _magn)
    {
        float gravity = Physics.gravity.y;

        //Calcul angle
        //tan²ø * g*Px² + tanø * Px * 2 * V0.mag² + (g*Px²-Py * 2 * V0.mag²)
        //x² * g * Px² + x * Px * 2 * V0.mag² + (g*Px²-Py * 2 * V0.mag²)
        float a = m_gravity * (Mathf.Pow(_targetPosition.x, 2));
        float b = _targetPosition.x * 2 * (Mathf.Pow(_magn, 2));
        float c = m_gravity * (Mathf.Pow(_targetPosition.x, 2)) - _targetPosition.y * 2 * (Mathf.Pow(_magn, 2));
        List<float> tanAngles = scndRadEquation.CalculateEquation(a, b, c);

        // angle in radians
        for (int i = 0; i < tanAngles.Count; i++)
        {
            tanAngles[i] = Mathf.Atan(tanAngles[i]);
        }
        return tanAngles;
    }

    public static List<float> GetAngleDeg(Vector3 _casterPosition, Vector3 _targetPosition, float _magn)
    {
        List<float> list = GetAngleRad(_casterPosition, _targetPosition, _magn);

        for (int i = 0; i < list.Count; i++)
        {
            list[i] *= Mathf.Rad2Deg;
        }
        return list;
    }




}
