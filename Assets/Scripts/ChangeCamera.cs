using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] private List<Camera> m_listCameras = new List<Camera>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1") && m_listCameras[0] != null)
        {
            SwitchCameras(0);
        }
        else if (Input.GetKeyDown("2") && m_listCameras[1] != null)
        {
            SwitchCameras(1);
        }
        else if (Input.GetKeyDown("3") && m_listCameras[2] != null)
        {
            SwitchCameras(2);
        }
    }

    private void SwitchCameras(int _keyNum)
    {
        for (int i = 0; i < m_listCameras.Count; i++)
        {
            if (m_listCameras[i] != null && _keyNum != i)
            {
                // turn camera off
                m_listCameras[i].GetComponent<Camera>().enabled = false;
            }
            else
            {
                // turn camera on
                m_listCameras[i].GetComponent<Camera>().enabled = true;
            }
        }
    }

}
