using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float camHeightFromGround = 0.6f;
    [SerializeField] private float restingHeight = 0.2f;
    [SerializeField] private Transform olympus;

    public void SetCamPosition(Milestone point, bool isResting = false)
    {
        Transform camPos = point.transform;

        RaycastHit hit;
        Vector3 heightMod = new Vector3(0f, isResting ? restingHeight : camHeightFromGround, 0f);
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
    public void LerpToCamPosition(Milestone point, bool isResting = false)
    {
        StartCoroutine(CamLerpRoutine(point, isResting));
    }
    private IEnumerator CamLerpRoutine(Milestone point, bool isResting)
    {
        Vector3 positionTarget;
        Vector3 start = this.transform.position;
        
        RaycastHit hit;
        if (Physics.Raycast(point.transform.position, Vector3.down, out hit, 200f))
        {
            positionTarget = hit.point + new Vector3(0f, isResting ? restingHeight : camHeightFromGround, 0f);
        }
        else
        {
            Debug.LogError("[CameraController] MapPoint not valid. Exiting");
            yield break;
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;

            this.transform.position = Vector3.Lerp(start, positionTarget, t);

            yield return null;
        }

        this.transform.position = positionTarget;
    }
    public void LookAt(Milestone point)
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
