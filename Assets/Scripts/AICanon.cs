using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class AICanon : MonoBehaviour
{
    [SerializeField] private GameObject m_caster = null;
    [SerializeField] private TargetMoving m_target = null;

    [SerializeField] private Slider m_angleSlider = null;
    [SerializeField] private Slider m_strengthSlider = null;
    [SerializeField] private Text m_angleTxt = null;
    [SerializeField] private Text m_strengthTxt = null;
    [SerializeField] private float m_magnitude = 10f;

    [SerializeField] private float m_time = 0f;
    [SerializeField] private float m_actualTime = 0f;

    [SerializeField] private bool m_isPredicate = false;

    [SerializeField] private LineRenderer m_lineRenderer = null;

    [SerializeField] float m_timeDebug = 5f;
    [SerializeField] [Range(0.01f, 10)] float m_precisionDebug = 0.2f;

    [SerializeField] private Rigidbody m_prefabProjectile = null;

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

        for (int i = 0; i < 30; i++)
        {
            m_listPos.Add(m_target.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetPosition();
        ShootWithTime();
    }

    #endregion

    List<float> CalculAngle()
    {
        /***create 2D repere***/
        //init Direction to shoot
        Vector3 shootDirection = m_target.transform.position - m_caster.transform.position;
        shootDirection.y = 0;
        shootDirection.Normalize();

        Vector2 initposition = new Vector2(m_caster.transform.position.x * shootDirection.x + m_caster.transform.position.z * shootDirection.z, transform.position.y);
        Vector2 targetPosition = new Vector2(m_target.transform.position.x * shootDirection.x + m_target.transform.position.z * shootDirection.z, m_target.transform.position.y);

        float Px = (targetPosition.x - initposition.x);
        float Py = (targetPosition.y - initposition.y);

        //Calcul angle
        //tan²ø * g*Px² + tanø * Px * 2 * V0.mag² + (g*Px²-Py * 2 * V0.mag²)
        //x² * g * Px² + x * Px * 2 * V0.mag² + (g*Px²-Py * 2 * V0.mag²)
        float a = m_gravity * (Mathf.Pow(Px, 2));
        float b = Px * 2 * (Mathf.Pow(m_strengthSlider.value, 2));
        float c = m_gravity * (Mathf.Pow(Px, 2)) - Py * 2 * (Mathf.Pow(m_strengthSlider.value, 2));
        List<float> tanAngles = scndRadEquation.CalculateEquation(a, b, c);

        // angle in radians
        for (int i = 0; i < tanAngles.Count; i++)
        {
            tanAngles[i] = Mathf.Atan(tanAngles[i]);
        }
        // angle in degrees
        for (int i = 0; i < tanAngles.Count; i++)
        {
            tanAngles[i] *= Mathf.Rad2Deg;
        }

        return tanAngles;

    }

    #region PRIVATE METHODS

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
        distance = (distance + targetVelocity) * (m_target.GetComponent<TargetMoving>().m_speed / Time.deltaTime);
        Vector3 Vo = ((m_target.transform.position - m_caster.transform.position - 0.5f * Physics.gravity * m_time * m_time) / (m_time));
        m_strength = Vo.magnitude;

        return Vo;
    }

    private Vector3 Predicate(float _time)
    {
        if (m_lineRenderer == null)
            return new Vector3();

        if (m_target == null)
            return new Vector3();

        Vector3 velocity = (m_listPos[0] - m_listPos[m_listPos.Count - 1]) / (Time.fixedDeltaTime * m_listPos.Count - 1);

        m_lineRenderer.positionCount = 2;

        Vector3 pos1 = m_listPos[1] + Vector3.up;
        Vector3 pos2 = m_listPos[1] + velocity * _time + Vector3.up;

        m_lineRenderer.SetPosition(0, pos1);
        m_lineRenderer.SetPosition(1, pos2);

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

        Vector3 velocity = Tool.GetVelocityWithTime(transform.position, Predicate(m_time), m_time);

        Spawn(velocity);
    }

    private void Spawn(Vector3 _velocity)
    {
        transform.forward = _velocity.normalized;
        m_actualTime = 0;
        Rigidbody projectile =
            Instantiate<Rigidbody>(m_prefabProjectile, m_caster.transform.position, m_caster.transform.rotation);
        projectile.velocity = _velocity;
    }

    #endregion
}
