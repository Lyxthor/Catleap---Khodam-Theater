using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private System.Func<Vector3> GetCameraFollowPositionFunc;
    public void Setup(System.Func<Vector3> GetCameraFollowPositionFunc)
    {
        
        this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
    }

    private void Update()
    {
        Vector3 CameraFollowPosition = GetCameraFollowPositionFunc();
        CameraFollowPosition.z = transform.position.z;

        Vector3 CameraMoveDir = (CameraFollowPosition - transform.position).normalized;
        float Distance = Vector3.Distance(CameraFollowPosition, transform.position);
        float CameraMoveSpeed = 2f;

        if (Distance > 0)
        {
            Vector3 NewCameraPosition = transform.position + CameraMoveDir * Distance * CameraMoveSpeed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(NewCameraPosition, CameraFollowPosition);

            if (distanceAfterMoving > Distance)
            {
                NewCameraPosition = CameraFollowPosition;
            }
            transform.position = NewCameraPosition;
        }


    }
}
