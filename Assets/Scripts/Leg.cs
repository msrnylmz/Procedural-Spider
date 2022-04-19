using UnityEngine;

public class Leg : MonoBehaviour
{
    public enum Direct { Right, Left }
    public Direct DirectLeg;
    [Range(0, 20)]
    [SerializeField] private float MovementSpeed;
    [Range(0, 20)]
    public float LegMovementOffsetY;
    [Space]
    public Transform Target;
    public Transform Hint;
    public Transform OriginOfRay;
    [Space]
    public Transform FirstBone;
    public Transform SecondBone;
    public Transform ThirdBone;
    [Space]
    public Vector3 NewPosition;
    public Vector3 OldPosition;
    public Vector3 CurrentPosition;
    [Space]
    public bool TargetReached;
    public bool MoveToTarget;
    public bool StartMove;

    private Spider m_Spider;
    private RaycastHit m_GroundRayHit;
    private float m_LerpTime;
    private float m_RayDistance;

    private Vector3 m_Direct;
    private Vector3 m_Origin;

    private void OnDrawGizmos()
    {
        if (m_Spider == null)
            return;
        float distance = Vector3.Distance(ThirdBone.position, FirstBone.position) + m_Spider.OriginOffset.y;
        Debug.DrawRay(OriginOfRay.position, m_Direct.normalized * distance, Color.red);
    }

    public void Initialize(Spider spider, Vector2 offset, float maxDistance)
    {
        m_Spider = spider;
        Vector3 originOffset = Vector3.zero;
        if (DirectLeg == Direct.Right)
        {
            originOffset = transform.position + (OriginOfRay.transform.right * offset.x) + (Vector3.up * offset.y);
        }
        else
        {
            originOffset = transform.position + (OriginOfRay.transform.right * -1 * offset.x) + (Vector3.up * offset.y);
        }

        //m_RayDistance = Vector3.Distance(ThirdBone.position, FirstBone.position) + m_Spider.OriginOffset.y;
        m_RayDistance = 100;

        OriginOfRay.position = originOffset;
        Ray(maxDistance, () => Target.position = NewPosition);
        OldPosition = NewPosition;
    }

    public bool Ray(float maxDistance, System.Action collision)
    {
        m_Direct = ((OriginOfRay.position + OriginOfRay.up * -20) + m_Spider.MovementDirection * 2) - OriginOfRay.position;
        m_Origin = OriginOfRay.position;
        if (Physics.Raycast(m_Origin, m_Direct, out m_GroundRayHit, m_RayDistance))
        {
            NewPosition = m_GroundRayHit.point;
            collision?.Invoke();
            return true;
        }
        return false;
    }
    public bool ControlLeg(int angleLimit)
    {
        return Mathf.Round(GetLegAngle()) >= angleLimit;
    }

    public float GetLegAngle()
    {
        Vector3 firstToSecond = FirstBone.position - SecondBone.position;
        Vector3 ThirdToSecond = ThirdBone.position - SecondBone.position;
        return Vector3.Angle(firstToSecond, ThirdToSecond);
    }


    public void Move(AnimationCurve legAnimation, System.Action Reached)
    {
        if (!MoveToTarget)
        {
            if (Ray(100, null))
            {
                MoveToTarget = true;
            }
        }
        else
        {
            if (m_LerpTime < 1)
            {
                float lerpRatio = m_LerpTime / 1;
                float positionOffset = legAnimation.Evaluate(lerpRatio) * LegMovementOffsetY;
                Vector3 tempPosition = Vector3.Lerp(OldPosition, NewPosition, m_LerpTime);
                tempPosition += m_Spider.transform.up * positionOffset;
                CurrentPosition = tempPosition;
                Target.position = CurrentPosition;
                m_LerpTime += Time.deltaTime * MovementSpeed;
            }
            else
            {
                OldPosition = NewPosition;
                TargetReached = true;
                StartMove = false;
                MoveToTarget = false;
                m_LerpTime = 0;
            }
        }
    }
}
