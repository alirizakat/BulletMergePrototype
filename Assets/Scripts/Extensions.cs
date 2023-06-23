using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions 
{
    public static bool TryGetRigidbodyComponent<T>(this Collider collider, out T component) where T : MonoBehaviour
    {
        if (collider.attachedRigidbody == null)
        {
            component = null;
            return false;
        }

        else
        {
            component = collider.attachedRigidbody.GetComponent<T>();
            return component != null;
        }
    }
}
