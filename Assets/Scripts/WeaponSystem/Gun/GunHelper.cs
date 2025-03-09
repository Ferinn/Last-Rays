using UnityEngine;
public static class GunHelper
{
    public static Vector2 AddAngle2Vector(Vector2 origin, float angle)
    {
        if (angle == 0) return origin;

        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        float newX = origin.x * cos - origin.y * sin;
        float newY = origin.x * sin + origin.y * cos;

        return new Vector2(newX, newY).normalized;
    }
}