using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class CanonShootAnticipate : MonoBehaviour
{
    [SerializeField] private GameObject m_caster = null;
    [SerializeField] private TargetMoving m_target = null;

    [SerializeField] private Slider m_angleSlider = null;
    [SerializeField] private Slider m_strengthSlider = null;
    [SerializeField] private Text m_angleTxt = null;
    [SerializeField] private Text m_strengthTxt = null;
    [SerializeField] private float m_magnitude = 10f;

    [SerializeField] private float m_time = 0f;

    [SerializeField] private LineRenderer m_lineRenderer = null;

    [SerializeField] float m_timeDebug = 5f;
    [SerializeField] [Range(0.01f, 10)] float m_precisionDebug = 0.2f;

    private float m_strength = 10f;
    private const float m_gravity = 9.81f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //m_angleSlider.maxValue = 90;
        //m_strengthSlider.maxValue = 100;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //m_strengthTxt.text = "Strength = " + m_strengthSlider.value.ToString();
        //m_angleTxt.text = "Angle = " + m_angleSlider.value.ToString();
        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.AddForce(Anticipate(), ForceMode.Impulse);
        }

        Predicate();
    }

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

    private void Predicate()
    {
        if (m_lineRenderer == null)
            return;

        List<Vector3> pos = new List<Vector3>();
        for (float i = 0; i < m_timeDebug; i += m_precisionDebug)
        {
            Vector3 vector3 = Tool.GetPredictionPositionWithGravity(transform.position, transform.forward * m_magnitude, i);
            pos.Add(vector3);
        }

        m_lineRenderer.positionCount = pos.Count;
        m_lineRenderer.SetPositions(pos.ToArray());

    }
}
