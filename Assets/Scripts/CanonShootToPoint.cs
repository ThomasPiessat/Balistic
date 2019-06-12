using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonShootToPoint : MonoBehaviour
{
    [SerializeField] private Rigidbody m_projectile;
    [SerializeField] private GameObject m_cursor;
    [SerializeField] private LayerMask m_layer;
    [SerializeField] private GameObject m_caster;

    [SerializeField] private LineRenderer m_lineRenderer;

    [Header("Values to change")]
    [SerializeField] private float m_time;

    private Camera m_cam;

    private const float m_gravity = 9.81f;

    // Start is called before the first frame update
    void Start()
    {
        m_cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        LaunchProjectile();
    }

    void LaunchProjectile()
    {
        Ray rayCam = m_cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(rayCam, out hit, 100f, m_layer))
        {
            m_cursor.SetActive(true);
            m_cursor.transform.position = hit.point + Vector3.up * 0.1f;
            Vector3 Vo = Tool.CalculateVelocity(hit.point, m_caster.transform.position, m_time);
            transform.rotation = Quaternion.LookRotation(Vo);
            DrawnLine(m_caster.transform.position, Vo, m_time, 0.1f);

            if (Input.GetMouseButtonDown(1))
            {
                Rigidbody rb = Instantiate(m_projectile, m_caster.transform.position, Quaternion.identity);
                rb.velocity = Vo;
            }
        }
        else
        {
            m_cursor.SetActive(false);
        }
    }

    //we give target point (rayCast)
    //=> found the velocity to reach the target
    Vector3 CalculateVelocity(Vector3 _target, Vector3 _caster, float _time)
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

    void DrawnLine(Vector3 _positionInitial, Vector3 _velocity, float _time, float _timePrecision)
    {
        List<Vector3> position = new List<Vector3>();

        for (float t = 0; t < _time; t += _timePrecision)
        {
            position.Add(_positionInitial + _velocity * t + 0.5f * Physics.gravity * t * t);
        }
        m_lineRenderer.positionCount = position.Count;
        m_lineRenderer.SetPositions(position.ToArray());
    }


}
