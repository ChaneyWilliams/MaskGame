using System.Runtime;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform target;
    void Move()
    {
        gameObject.transform.position += target.position;
    }
}
