using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float camHeightFromGround = 0.6f;
    [SerializeField] private Transform olympus;

    public void SetCamPosition(MapPoint point)
    {
        Transform camPos = point.transform;

        RaycastHit hit;
        Vector3 heightMod = new Vector3(0f, camHeightFromGround, 0f);
        if (Physics.Raycast(camPos.position, Vector3.down, out hit, 200f)) 
        { 
            this.transform.position = hit.point;
            this.transform.position += heightMod;
        }
        else
        {
            Debug.LogError("[CamController] Raycast to ground failed");
            this.transform.position = camPos.position;
        }
        this.transform.LookAt(olympus);
    }
    public void LookAt(MapPoint point)
    {
        if (!point) { return; }

        Vector3 lookTarget = olympus.position;

        RaycastHit hit;
        Vector3 heightMod = new Vector3(0f, camHeightFromGround, 0f);
        if (Physics.Raycast(point.transform.position, Vector3.down, out hit, 200f))
        {
            lookTarget = hit.point + heightMod;
        }

        this.transform.LookAt(lookTarget);
    }
}
