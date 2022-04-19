using UnityEngine;

public class Spider : FourLegged
{
    [HideInInspector] public int MovementSpeed;
    [HideInInspector] public Vector3 MovementDirection;
    [Space]
    public AnimationCurve LegAnimation;
    [Range(1, 50)]
    public int RotateMovementSpeed;
    [Range(1, 50)]
    public int NormalMovementSpeed;
    [Range(1, 50)]
    public int SprintMovementSpeed;

    [Space]
    public Vector2 OriginOffset;
    public float SpiderOffset;
    public int LegAngleLimit;
    [Space]
    public bool IsSprinting;
    public bool IsMoving;
    public bool IsRotatinRight;
    public bool IsRotatinLeft;
    [Space]
    public bool LegMoveOrderForSpiderMovement;

    public bool IsRotating
    {
        get
        {
            return IsRotatinRight || IsRotatinLeft;
        }
    }

    private int m_LegMovementOrderForRotate;

    public override void Initialize(Spider spider, Vector2 offset, float maxDistance)
    {
        base.Initialize(spider, offset, maxDistance);
        MovementSpeed = NormalMovementSpeed;
        m_LegMovementOrderForRotate = 1;
        LegMoveOrderForSpiderMovement = false;
    }

    private void Update()
    {
        InputControl();
        Rotate();
        Movement();
        LegMovementControl();
        FirstCrossedLegs.Move(LegAnimation);
        SecondCrossedLegs.Move(LegAnimation);
    }
    private void LegMovementControl()
    {
        if (SecondCrossedLegs.GetTargetReached() && FirstCrossedLegs.GetTargetReached())
        {
            if (IsRotating && !IsMoving)
            {
                if (IsRotatinRight)
                {
                    StartedMoveControl(FirstCrossedLegs, SecondCrossedLegs);
                }
                else if (IsRotatinLeft)
                {
                    StartedMoveControl(SecondCrossedLegs, FirstCrossedLegs);
                }
            }
            else if (IsMoving)
            {
                if (!FirstCrossedLegs.GetStartMove() && FirstCrossedLegs.Control(LegAngleLimit) && LegMoveOrderForSpiderMovement)
                {
                    StartedMove(FirstCrossedLegs);
                }
                else if (!SecondCrossedLegs.GetStartMove() && SecondCrossedLegs.Control(LegAngleLimit) && !LegMoveOrderForSpiderMovement)
                {
                    StartedMove(SecondCrossedLegs);
                }
            }
        }
    }

    private void StartedMove(TwoCrossedLegs twoCrossedLegs)
    {
        twoCrossedLegs.SetStartMove(true);
        twoCrossedLegs.SetTargetReached(false);
        LegMoveOrderForSpiderMovement = !LegMoveOrderForSpiderMovement;
    }

    private void StartedMoveControl(TwoCrossedLegs first, TwoCrossedLegs second)
    {
        if (!first.GetStartMove() && first.Control(LegAngleLimit))
        {
            StartedMove(first);
            m_LegMovementOrderForRotate++;
        }
        else if (m_LegMovementOrderForRotate % 2 == 0 && first.GetTargetReached())
        {
            StartedMove(second);
            m_LegMovementOrderForRotate = 1;
        }
    }
    public float horizontal;
    public float vertical;

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        MovementDirection = (transform.forward * vertical + transform.right * horizontal);

        if (MovementDirection.magnitude != 0)
        {
            Vector3 spiderPosition = GetNewPosition() + transform.up * SpiderOffset;
            Vector3 targetPosition = spiderPosition + (MovementDirection * 10);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * MovementSpeed);
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }

    private void Rotate()
    {
        float direct = 0;
        if (IsRotatinLeft)
            direct = -1;
        else if (IsRotatinRight)
            direct = 1;

        float angleY = transform.eulerAngles.y + (direct * MovementSpeed);
        Vector3 targetAngle = new Vector3(GetEulerValue().x, transform.eulerAngles.y, GetEulerValue().z);
        Quaternion targetRot = Quaternion.Euler(targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, MovementSpeed * Time.deltaTime);
    }

    private void InputControl()
    {
        // Origin Rotates
        if (Input.GetKey(KeyCode.Q))
        {
            IsRotatinLeft = true;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            IsRotatinRight = true;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            IsRotatinRight = false;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            IsRotatinLeft = false;
        }

        // MovementSpeed Changes
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!IsSprinting)
            {
                ChangeMovementSpeed();
                IsSprinting = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ChangeMovementSpeed();
            IsSprinting = false;
        }
    }

    private void ChangeMovementSpeed()
    {
        MovementSpeed = MovementSpeed == NormalMovementSpeed ? SprintMovementSpeed : NormalMovementSpeed;
    }
    private void Start()
    {
        Initialize(this, OriginOffset, 100);
    }
}
