using UnityEngine;

public class ballTowardsRayCast : MonoBehaviour
{

    public Camera cam;
    public GameObject lookReference;
    public GameObject modelOrientation;
    public LayerMask layersToIgnore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100f;
        mousePos = cam.ScreenToViewportPoint(mousePos);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, ~layersToIgnore))
        {
            lookReference.transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Vector3 lookDirection = lookReference.transform.position - modelOrientation.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            modelOrientation.transform.rotation = lookRotation;
        }
        Debug.DrawRay(modelOrientation.transform.position, modelOrientation.transform.forward * 10, Color.red);
    }
}
