using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] float rotationSpeed = 400f;
    [SerializeField] float slideSpeed = 5f;
    [SerializeField] float sinkSpeed = 1f;
    [SerializeField] Transform cameraTarget;

    Queue<Vector3> commands = new Queue<Vector3>();

    Vector3 startPointForRay;
    Vector3 movementDirection;
    Vector3 rotationAxis;
    Vector3 rotationPoint;
    Vector3 startPosition;
    Vector3 endPosition;
    Quaternion startRotation;
    Quaternion endRotation;

    float pointY;
    float angle;
    float timer;
    bool isRotating = false;
    bool isSliding = false;

    #endregion



    #region Properties

    public Transform CameraTarget
    {
        get
        {
            return cameraTarget;
        }
    }

    #endregion



    #region Unity Lifecycle

    void OnEnable()
    {
        InputManager.Instance.OnSwipe += AddCommand;
    }


    void Awake()
    {
        pointY = cameraTarget.position.y; 
    }


    void Update()
    {
        if (!isRotating)
        {
            if (IsInLava())
            {
                Sink();
            }
            else if (isSliding)
            {
                Slide();
            }
            else
            {
                if (commands.Count > 0)
                {
                    SetRotation();
                }
            }
        }
        else
        {
            Rotate();
        }

        Vector3 position = transform.position;

        position.y = pointY;
        cameraTarget.position = position;
        cameraTarget.rotation = Quaternion.identity;
    }


    void OnDisable()
    {
        InputManager.Instance.OnSwipe -= AddCommand;
    }

    #endregion



    #region Public Methods


    public void ResetPlayer(Quaternion rotation)
    {
        isRotating = false;
        isSliding = false;

        playerRigidbody.isKinematic = true;
        playerRigidbody.useGravity = false;

        transform.position = new Vector3(0f, transform.localScale.y / 2, 0f);
        transform.rotation = rotation;

        if (commands.Count > 0)
        {
            commands.Clear();
        }
    }

    #endregion



    #region Private Methods

    void SetRotation()
    {
        RaycastHit hit;

        if (commands.Count > 0)
        {
            movementDirection = commands.Dequeue();
        }

        startPointForRay = transform.position + movementDirection * transform.localScale.x;

        if (Physics.Raycast(transform.position, movementDirection, out hit, 1) && hit.transform.tag == "Wall")
        {
            AudioManager.Instance.PlaySFXClip(ClipName.RotationDenied);
        }
        else
        {
            if (Physics.Raycast(startPointForRay, Vector3.down, out hit, 1) && hit.transform.tag != "Wall")
            {
                angle = 90f;
            }
            else
            {
                angle = 45f;

                AudioManager.Instance.PlaySFXClip(ClipName.RotationDenied);
            }

            rotationAxis = Vector3.Cross(Vector3.up, movementDirection);
            rotationPoint = (
                transform.position + Vector3.down * (transform.localScale.x / 2)) + 
                movementDirection * (transform.localScale.x / 2);

            startRotation = transform.rotation;
            endRotation = Quaternion.AngleAxis(angle, rotationAxis);

            startPosition = transform.position;
            endPosition = transform.position + movementDirection * transform.localScale.x;

            timer = 0.0f;
            isRotating = true;
        }
    }


    void SetSlide()
    {
        endPosition = transform.position + movementDirection;
        isSliding = true;

        InputManager.Instance.IsEnabled = false;
        commands.Clear();
    }


    void Rotate()
    {
        timer += Time.deltaTime;

        if (timer < angle / rotationSpeed)
        {
            transform.RotateAround(rotationPoint, rotationAxis, rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (angle == 90f)
            {
                if (!IsInLava())
                {
                    AudioManager.Instance.PlayRegularSFXClip(ClipName.RotationEnded);
                }

                transform.position = endPosition;
                transform.rotation = endRotation;

                if (IsOnIce())
                {
                    SetSlide();
                }

                isRotating = false;

                return;
            }

            if (timer < 2 * angle / rotationSpeed)
            {
                transform.RotateAround(rotationPoint, -rotationAxis, rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = startPosition;
                transform.rotation = startRotation;

                isRotating = false;
                return;
            }
        }
    }


    void Slide()
    {
        if (transform.position != endPosition)
        {
            RaycastHit hitOnIce;
            while (Physics.Raycast(endPosition, Vector3.down, out hitOnIce, 1) &&
                hitOnIce.transform.tag == "Ice")
            {
                endPosition = endPosition + movementDirection;
            }
            endPosition -= movementDirection;
            transform.position = Vector3.MoveTowards(
                transform.position, endPosition, slideSpeed * Time.deltaTime);
        }
        else
        {
            if (Physics.Raycast(endPosition + movementDirection, Vector3.down, 1))
            {
                SetRotation();
                isSliding = false;
                InputManager.Instance.IsEnabled = true;
            }
        }
    }


    void Sink()
    {
        transform.Translate(Vector3.down * Time.deltaTime * sinkSpeed, Space.World);
    }


    void AddCommand(Vector3 dir)
    {
        if (commands.Count < 1)
        {
            commands.Enqueue(dir);
        }
    }


    bool IsOnIce()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            if (hit.transform.tag == "Ice")
            {
                return true;
            }
        }

        return false;
    }


    bool IsInLava()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            if (hit.collider.tag == "Lava")
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}
