using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeExitRotation : MonoBehaviour
{
    public Transform parent;

    void LateUpdate()
    {
        parent = transform.parent;

        // Just set the child's local Z to the opposite of the parent's world Z
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            -parent.eulerAngles.z
        );
    }
}
