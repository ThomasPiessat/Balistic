using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class AICanon : MonoBehaviour
{
    [SerializeField] private GameObject m_caster = null;
    [SerializeField] private Target m_target = null;

    [SerializeField] private Rigidbody m_prefabProjectile = null;

    [SerializeField] private float m_distanceToShoot= 0f;
    [SerializeField] private float m_time = 0;
    [SerializeField] private float m_actualTime = 0f;
    [SerializeField] private float m_fireRate = 0f;

    [SerializeField] private bool m_isPredicate = false;

    [SerializeField] private List<Vector3> m_listPos = new List<Vector3>();

    private float m_strength = 10f;
    private const float m_gravity = 9.81f;

    #region MONOBEHAVIOUR METHODS

    // Start is called before the first frame update
    void Start()
    {
        if (m_target == null)
        {
            return;
        }

        m_target.GetComponent<Rigidbody>();

        for (int i = 0; i < 30; i++)
        {
            m_listPos.Add(m_target.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetPosition();
        if (CanShoot())
        {
            if (!IsInRange())
            {
                return;
            }
            ShootWithTime();
        }
        else
        {
            m_actualTime += Time.deltaTime;
        }
        
    }

    #endregion

    #region PRIVATE METHODS


    bool CanShoot()
    {
        return m_actualTime >= m_fireRate;
    }

    bool IsInRange()
    {
        if (m_target.transform == null)
            return false;
        else
            return (m_target.transform.position - transform.position).magnitude < m_distanceToShoot;
    }

    void UpdateTargetPosition()
    {
        if (m_target.transform)
        {
            for (int i = 0; i < m_listPos.Count - 1; i++)
            {
                m_listPos[i] = m_listPos[i + 1];
            }

            m_listPos[m_listPos.Count - 1] = m_target.transform.position;
        }
    }


    private Vector3 Anticipate()
    {
        //divide detlaTime to get m/s

        //Px(t) = Pox + Vox * t + ½ * Ax * t² 	= Pox + Vox * t 	// Ax = 0
        //Py(t) = Poy + Voy * t + ½ *Ay * t² 
        //Pz(t) = Poz + Voz * t + ½ *Az * t² 	= Poz + Voz * t 	// Az = 0

        //Vector3 TargetVelocity = Tool.CalculateVelocity(m_target.transform.position, m_caster.transform.position, m_time);

        //Vector3 Vo = ((m_target.transform.position - m_caster.transform.position + 0.5f * Physics.gravity * m_time * m_time) / (m_time));

        Vector3 distance = m_target.transform.position - m_caster.transform.position;
        Vector3 targetVelocity = Tool.CalculateVelocity(m_target.transform.position, m_caster.transform.position, m_time);
        distance = (distance + targetVelocity) * (m_target.GetComponent<Target>().m_speed / Time.deltaTime);
        Vector3 Vo = ((m_target.transform.position - m_caster.transform.position - 0.5f * Physics.gravity * m_time * m_time) / (m_time));
        m_strength = Vo.magnitude;

        return Vo;
    }

    private Vector3 Predicate(float _time)
    {
        if (m_target == null)
            return new Vector3();

        Vector3 velocity = (m_listPos[0] - m_listPos[m_listPos.Count - 1]) / (Time.fixedDeltaTime * m_listPos.Count - 1);

        Vector3 pos1 = m_listPos[1] + Vector3.up;
        Vector3 pos2 = m_listPos[1] + velocity * _time + Vector3.up;

        return m_target.transform.position + velocity * _time + Vector3.up;
    }

    private void ShootWithTime()
    {
        if (m_prefabProjectile == null || m_target.transform == null)
        {
            return;
        }

        Vector3 positionToShoot = new Vector3();

        if (m_isPredicate)
        {
            positionToShoot = Predicate(m_time);
        }
        else
        {
            positionToShoot = m_target.transform.position;
        }

        Vector3 velocity = Tool.GetVelocityWithTime(transform.position, positionToShoot, m_time);

        Spawn(velocity);
        //List<Vector3> pos = new List<Vector3>();
        //float precision = 0.2f;
        //for (float i = 0; i < m_time + precision; i += precision)
        //{
        //    pos.Add(Tool.GetPredictionPositionWithGravity(transform.position, velocity, i));
        //}
    }

    private void Spawn(Vector3 _velocity)
    {
        transform.forward = _velocity.normalized;
        m_actualTime = 0;
        Rigidbody projectile = Instantiate<Rigidbody>(m_prefabProjectile, m_caster.transform.position, m_caster.transform.rotation);
        projectile.velocity = _velocity;
    }

    #endregion
}
