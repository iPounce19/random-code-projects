using UnityEngine;

public class player_Trigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}
