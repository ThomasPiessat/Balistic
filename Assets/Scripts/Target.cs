using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Transform[] m_wayPoint;
    [SerializeField] public float m_speed;
    [SerializeField] private bool m_isMoving;

    private int m_currentWayPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_isMoving == true)
        {
            Move();
        }
        else
            return;
    }

    public void Moving()
    {
        if (m_isMoving == true)
            m_isMoving = false;
        else
            m_isMoving = true;
    }


    public void Move()
    {
        Rigidbody rb = transform.GetComponent<Rigidbody>();
        Vector3 moveDir = m_wayPoint[m_currentWayPoint].position - transform.position;

        if (Vector3.Distance(transform.position, m_wayPoint[m_currentWayPoint].position) <= 1)
        {
            if (m_currentWayPoint + 1 < m_wayPoint.Length)
            {
                m_currentWayPoint++;
            }

            else
            {
                m_currentWayPoint = 0;
            }

        }
        Vector3 nextPos = Vector3.MoveTowards(transform.position, m_wayPoint[m_currentWayPoint].position, m_speed * Time.deltaTime);
        rb.MovePosition(nextPos);
    }
}
