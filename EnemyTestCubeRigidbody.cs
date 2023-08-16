using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestCubeRigidbody : MonoBehaviour
{

    public Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.isKinematic = true;
    }
}
