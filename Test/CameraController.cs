using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cam;


    // Update is called once per frame
    void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(currentRotation.x, cam.rotation.eulerAngles.y, currentRotation.z);
        transform.rotation = Quaternion.Euler(newRotation);
    }
}
