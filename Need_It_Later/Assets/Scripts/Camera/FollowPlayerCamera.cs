using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [Foldout("Follow Camera Parameters", foldEverything = true, styled = true, readOnly = false)]
    public float FollowSpeed;
    
    [Foldout("Follow Camera References", foldEverything = true, styled = true, readOnly = false)]
    public Camera TargetCamera;
    public Transform PlayerTransform;
    public Transform MaxBoundary;
    public Transform MinBoundary;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var targetPos = new Vector3(
            Mathf.Clamp(PlayerTransform.position.x, MinBoundary.position.x, MaxBoundary.position.x),
            Mathf.Clamp(PlayerTransform.position.y, MinBoundary.position.y, MaxBoundary.position.y),
            this.transform.position.z);
        TargetCamera.transform.position = Vector3.MoveTowards(
            TargetCamera.transform.position,
            targetPos,
            FollowSpeed);
    }
}
