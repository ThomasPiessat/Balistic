using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody m_prefab;
    [SerializeField] private GameObject m_target;
    private Transform m_targetTransform;
    private Rigidbody m_targetRigidbody;
    [SerializeField] private float m_time;

    List<Vector3> m_listPos = new List<Vector3>();

    private void Start()
    {
        m_targetTransform = m_target.transform;
        m_targetRigidbody = m_targetRigidbody.GetComponent<Rigidbody>();

        for (int i = 0; i < 30; i++)
        {
            m_listPos.Add(m_targetTransform.position);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Shoot();
        }
    }

    Vector3 PredictPosition(float _time)
    {
        if (m_target == null)
        {
            return new Vector3();
        }

        Vector3 Vo = (m_listPos[0] - m_listPos[m_listPos.Count - 1]) / (Time.fixedDeltaTime * m_listPos.Count - 1);

        Vector3 pos1 = m_listPos[1] + Vector3.up;
        Vector3 pos2 = m_listPos[1] + Vo * _time + Vector3.up;

        return m_targetTransform.position + Vo * _time + Vector3.up;
    }

    void Shoot()
    {
        Vector3 posToShoot = new Vector3();
        posToShoot = PredictPosition(m_time);
        Vector3 Vo = Tool.CalculateVelocity(posToShoot, transform.position, m_time);
    }

    void SpawnAndVelocity(Vector3 _velocity)
    {
        transform.forward = _velocity;
        Rigidbody projectile = Instantiate<Rigidbody>(m_prefab, transform.position, Quaternion.identity);
        projectile.velocity = _velocity;
    }
}
