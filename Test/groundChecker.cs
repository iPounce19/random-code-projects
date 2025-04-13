using UnityEngine;

public class groundChecker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            Debug.Log(transform.name + ": Grounded");
        }
    }
}
