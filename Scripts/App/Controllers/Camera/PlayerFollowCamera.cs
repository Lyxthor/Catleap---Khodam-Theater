using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    // ROOM CAMERA
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    // FOLLOW PLAYER
    [SerializeField] private Transform player;
    // FOLLOW PLAYER : LOOK AHEAD
    [SerializeField] private float AheadDistance;
    [SerializeField] private float CameraSpeed;
    private float lookAhead;

    private void Update()
    {
        // MOVE TO NEWROOM CAMERA CONTROLLER
        /*transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);*/

        // FOLLOW PLAYER
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);

        // FOLLOW PLAYER : LOOK AHEAD
        /*transform.position = new Vector3(player.position.x + lookAhead, player.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (AheadDistance + player.localScale.x), Time.deltaTime * CameraSpeed);*/
    }
    // MOVE TO NEWROOM CAMERA CONTROLLER
    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
