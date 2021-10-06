// Values are adjusted for a top-down view

using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Transform target;

    [SerializeField, Range(0.01f, 1f)] float smoothTime = 0.125f;
    [Tooltip("Camera's location relative to the player")]
    [SerializeField] Vector3 offset = new Vector3(0, 1, -10);
    [SerializeField] float cameraSpeed = 0.125f;

    [SerializeField] Vector2 cameraRange = new Vector2(5f, 35f);

    // toggle tracking
    [SerializeField] bool lookAtPlayer = true;
    [SerializeField] bool smoothFollowing = true;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        if (Input.GetKey(KeyCode.N) && offset.y < cameraRange.x) offset.y += cameraSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.M) && offset.y > cameraRange.y) offset.y -= cameraSpeed * Time.deltaTime;

        if (smoothFollowing){
            transform.position = Vector3.SmoothDamp(
                transform.position, desiredPosition, ref velocity, smoothTime);
        }

        if (lookAtPlayer) {
            transform.LookAt(target);
        }
    }
}