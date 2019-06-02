using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] private List<Camera> cameras = new List<Camera>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1") && cameras[0] != null)
        {
            SwitchCameras(0);
        }
        else if (Input.GetKeyDown("2") && cameras[1] != null)
        {
            SwitchCameras(1);
        }
        else if (Input.GetKeyDown("3") && cameras[2] != null)
        {
            SwitchCameras(2);
        }
    }

    private void SwitchCameras(int keyNum)
    {
        for (int i = 0; i < cameras.Count - 1; i++)
        {
            if (cameras[i] != null && keyNum != i)
            {
                // turn camera off
                cameras[i].GetComponent<Camera>().enabled = false;
            }
            else
            {
                // turn camera on
                cameras[i].GetComponent<Camera>().enabled = true;
            }
        }
    }

}
