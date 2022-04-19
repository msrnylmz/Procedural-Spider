using UnityEngine;

public class FourLegged : MonoBehaviour
{
    [SerializeField] protected TwoCrossedLegs FirstCrossedLegs;
    [SerializeField] protected TwoCrossedLegs SecondCrossedLegs;

    protected Leg[] Legs = new Leg[4];
    protected Spider m_Spider;

    private float m_AngleX;
    private float m_AngleZ;
    private Vector3 m_FirstX;
    private Vector3 m_SecondX;
    private Vector3 m_FirstZ;
    private Vector3 m_SecondZ;
    public virtual void Initialize(Spider spider, Vector2 offset, float maxDistance)
    {
        m_Spider = spider;
        SetLegs();
        FirstCrossedLegs.Initialize(spider, offset, maxDistance);
        SecondCrossedLegs.Initialize(spider, offset, maxDistance);
    }

    private void SetLegs()
    {
        Legs[0] = SecondCrossedLegs.Legs[0];
        Legs[1] = FirstCrossedLegs.Legs[0];
        Legs[2] = FirstCrossedLegs.Legs[1];
        Legs[3] = SecondCrossedLegs.Legs[1];
    }
    public Vector3 GetEulerValue()
    {
        m_FirstX = (Legs[0].Target.position + Legs[1].Target.position) / 2;
        m_SecondX = (Legs[2].Target.position + Legs[3].Target.position) / 2;

        m_FirstZ = (Legs[0].Target.position + Legs[2].Target.position) / 2;
        m_SecondZ = (Legs[1].Target.position + Legs[3].Target.position) / 2;


        m_AngleX = Utility.CalculateAngleX(Vector3.up, m_FirstX, m_SecondX, 90);
        m_AngleZ = Utility.CalculateAngleZ(Vector3.up, m_FirstZ, m_SecondZ, -90);

        float x;
        float z;

        if (m_FirstX.y <= m_SecondX.y) { x = m_AngleX; }
        else { x = m_AngleX * -1; }

        if (m_FirstZ.y <= m_SecondZ.y) { z = m_AngleZ * -1; }
        else { z = m_AngleZ; }

        return new Vector3(x, 0, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_FirstX, 1);
        Gizmos.DrawWireSphere(m_SecondX, 1);
        Gizmos.DrawLine(m_FirstX, m_SecondX);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_FirstZ, 1);
        Gizmos.DrawWireSphere(m_SecondZ, 1);
        Gizmos.DrawLine(m_FirstZ, m_SecondZ);
    }

    public Vector3 GetNewPosition()
    {
        Vector3 result = Vector3.zero;
        foreach (var Leg in Legs)
        {
            result += Leg.NewPosition;
        }
        result = result / Legs.Length;
        return result;
    }

    public Vector3 GetOldPosition()
    {
        Vector3 result = Vector3.zero;
        foreach (var Leg in Legs)
        {
            result += Leg.OldPosition;
        }
        result = result / Legs.Length;
        return result;
    }
}
