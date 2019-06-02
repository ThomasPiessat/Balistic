using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Caster : MonoBehaviour
{
    [SerializeField] public float m_minfireStrength;
    [SerializeField] public float m_maxFireStrength;
    [SerializeField] private Slider m_slider;
    [SerializeField] public Slider m_strengthSlider;

    private float m_timerBegin;
    public float m_speed;

    // Start is called before the first frame update
    void Start()
    {
        m_speed = 2f;
        m_timerBegin = 0f;
        m_strengthSlider.maxValue = m_maxFireStrength;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangeRotation()
    {
        //transform.rotation = Quaternion.Euler( 0, 0, m_slider.value * -1);
        float h = m_speed * Input.GetAxis("Mouse X");
        float v = m_speed * Input.GetAxis("Mouse Y");

        transform.Rotate(-v, h, 0);
    }

    void ChangeForce()
    {
        if (Input.GetMouseButton(0))
        {
            m_timerBegin += 200 * Time.deltaTime;
            m_strengthSlider.value = m_timerBegin;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_timerBegin = 0;
            m_strengthSlider.value = m_timerBegin;
        }
    }

}
