using UnityEngine;

public class Utility : MonoBehaviour
{
    #region Angle
    public static float CalculateAngleX(Vector3 tr, Vector3 from, Vector3 to, float defaultAngle)
    {
        return Quaternion.FromToRotation(tr, to - from).eulerAngles.x - defaultAngle;
    }

    public static float CalculateAngleZ(Vector3 tr, Vector3 from, Vector3 to, float defaultAngle)
    {
        return Quaternion.FromToRotation(tr, to - from).eulerAngles.z - defaultAngle;
    }

    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    public static Vector3 GetAngleRay(Vector3 direct, Vector3 normal)
    {
        return Quaternion.FromToRotation(direct, normal).eulerAngles;
    }
    #endregion

    #region 
    public static Vector3 Multiply(Vector3 first, Vector3 second)
    {
        return new Vector3(first.x * second.x, first.y * second.y, first.z * second.z);
    }
    #endregion
}
