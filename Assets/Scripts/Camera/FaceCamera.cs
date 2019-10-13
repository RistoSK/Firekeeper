using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private void OnEnable()
    {
        transform.LookAt(CameraFollow.MainCamera.transform);
        Vector3 eulers = transform.eulerAngles;
        eulers.x = 0;
        eulers.z = 0;
        transform.eulerAngles = eulers;
    }
}
