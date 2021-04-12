using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
	[SerializeField] private Transform transformToLookAt;

    void Update()
    {
        transform.LookAt(transformToLookAt.position);
    }
}
