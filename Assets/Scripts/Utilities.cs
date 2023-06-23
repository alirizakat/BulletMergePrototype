using UnityEngine;

public static class Utilities
{
    public static float Decay(float from, float to, float lambda, float dt)
    {
        if (Mathf.Approximately(from, to)) return to;
        else return Mathf.Lerp(from, to, 1f - Mathf.Exp(-lambda * dt));
    }

    public static Vector3 Decay(Vector3 from, Vector3 to, float lambda, float dt)
    {
        Vector3 decayed = new Vector3(
            Decay(from.x, to.x, lambda, dt),
            Decay(from.y, to.y, lambda, dt),
            Decay(from.z, to.z, lambda, dt)
            ) ;

        return decayed;
    }
}
