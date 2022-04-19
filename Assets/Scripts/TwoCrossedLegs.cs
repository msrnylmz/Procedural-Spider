using UnityEngine;

public class TwoCrossedLegs : MonoBehaviour
{
    public Leg[] Legs = new Leg[2];

    public void SetTargetReached(bool cont)
    {
        Legs[0].TargetReached = cont;
        Legs[1].TargetReached = cont;
    }

    public bool GetTargetReached()
    {
        return Legs[0].TargetReached && Legs[1].TargetReached;
    }

    public void SetStartMove(bool cont)
    {
        Legs[0].StartMove = cont;
        Legs[1].StartMove = cont;
    }

    public bool GetStartMove()
    {
        return Legs[0].StartMove && Legs[1].StartMove;
    }
    public void Initialize(Spider spider, Vector2 offset, float maxDistance)
    {
        foreach (var Leg in Legs)
        {
            Leg.Initialize(spider, offset, maxDistance);
        }
    }

    public void Move(AnimationCurve legAnimation)
    {
        if (GetStartMove())
        {
            Legs[0].Move(legAnimation, null);
            Legs[1].Move(legAnimation, null);
        }
    }

    public bool Control(int angleLimit)
    {
        if (Legs[0].ControlLeg(angleLimit) || Legs[1].ControlLeg(angleLimit))
        {
            return true;
        }
        return false;
    }
}
