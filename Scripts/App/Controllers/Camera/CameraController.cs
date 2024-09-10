using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    //[SerializeField] private CameraFollow cameraFollow;
    private Vector3 cameraFollowPosition;
    private Vector3 currentCameraPosition;
    private float CameraMoveSpeed = 2f;
    // Start is called before the first frame update
    /* private void Start()
     {
         cameraFollow.Setup(() => cameraFollowPosition);
     }*/

    // Update is called once per frame
    private void Update()
    {
        float moveAmount = 5f;
        if (Input.GetKey(KeyCode.W))
            cameraFollowPosition.y += moveAmount * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            cameraFollowPosition.y -= moveAmount * Time.deltaTime;
        else if (Input.GetKey(KeyCode.D))
            cameraFollowPosition.x += moveAmount * Time.deltaTime;
        else if (Input.GetKey(KeyCode.A))
            cameraFollowPosition.x -= moveAmount * Time.deltaTime;
        GetCameraFollowPosition();
    }
    private void GetCameraFollowPosition()
    {
        //if (Vector3.Distance(currentCameraPosition, cameraFollowPosition) < 0.0001) return;
        currentCameraPosition = cameraFollowPosition;
        //Vector3 cameraFollowPosition = cameraFollowPosition;
        cameraFollowPosition.z = transform.position.z;

        Vector3 CameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        float Distance = Vector3.Distance(cameraFollowPosition, transform.position);
        

        if (Distance > 0)
        {
            Vector3 NewCameraPosition = transform.position + CameraMoveDir * Distance * CameraMoveSpeed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(NewCameraPosition, cameraFollowPosition);

            if (distanceAfterMoving > Distance)
            {
                NewCameraPosition = cameraFollowPosition;
            }
            transform.position = NewCameraPosition;
        }
    }
    private void SetY(float y)
    {

    }
    private void SetPosition(float x, float y, float z)
    {

    }
}
