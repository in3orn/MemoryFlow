using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float velocity;

    void Update()
    {
        Vector3 diff = target.transform.position + offset - transform.position;
        if (diff.magnitude > velocity)
        {
            transform.position += diff.normalized * velocity;
        }
        else
        {
            transform.position = target.transform.position + offset;
        }
    }
}
