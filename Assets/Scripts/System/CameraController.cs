//===========================================================================//
//                  Based on FreeFlyCamera (Version 1.2)                     //
//                        (c) 2019 Sergey Stafeyev                           //
//===========================================================================//

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    #region UI

    [Space]

    [SerializeField]
    [Tooltip("The script is currently active")]
    private bool _active = true;

    [Space]

    [SerializeField]
    [Tooltip("Camera rotation by mouse movement is active")]
    private bool _enableRotation = true;

    [SerializeField]
    [Tooltip("Sensitivity of mouse rotation")]
    private float _mouseSense = 1.8f;

    [Space]

    [SerializeField]
    [Tooltip("Camera zooming in/out by 'Mouse Scroll Wheel' is active")]
    private bool _enableTranslation = false;

    [SerializeField]
    [Tooltip("Velocity of camera zooming in/out")]
    private float _translationSpeed = 55f;

    [Space]

    [SerializeField]
    [Tooltip("Camera movement by 'W','A','S','D','Q','E' keys is active")]
    private bool _enableMovement = true;

    [SerializeField]
    [Tooltip("Camera movement speed")]
    private float _movementSpeed = 10f;

    [SerializeField]
    [Tooltip("Speed of the quick camera movement when holding the 'Left Shift' key")]
    private float _boostedSpeed = 50f;

    [SerializeField]
    [Tooltip("Boost speed")]
    private KeyCode _boostSpeed = KeyCode.LeftShift;

    [SerializeField]
    [Tooltip("Move up")]
    private KeyCode _moveUp = KeyCode.E;

    [SerializeField]
    [Tooltip("Move down")]
    private KeyCode _moveDown = KeyCode.Q;

    [Space]

    [SerializeField]
    [Tooltip("Acceleration at camera movement is active")]
    private bool _enableSpeedAcceleration = false;

    [SerializeField]
    [Tooltip("Rate which is applied during camera movement")]
    private float _speedAccelerationFactor = 1.5f;

    [Space]

    [SerializeField]
    [Tooltip("This keypress will move the camera to initialization position")]
    private KeyCode _initPositonButton = KeyCode.R;

    #endregion UI

    private CursorLockMode _wantedMode;

    private float _currentIncrease = 1;
    private float _currentIncreaseMem = 0;

    private Vector3 _initPosition;
    private Vector3 _initRotation;

    [SerializeField] private Transform _body;
    [SerializeField] private Transform _orientation;
    private bool _freefly = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_boostedSpeed < _movementSpeed)
            _boostedSpeed = _movementSpeed;
    }
#endif


    private void Start()
    {
        _initPosition = _body.position;
        _initRotation = transform.eulerAngles;

        SetConstrainMode();
    }

    private void OnEnable()
    {
        if (_active)
            _wantedMode = CursorLockMode.Locked;
    }

    private void SetConstrainMode()
    {
        _freefly = false;
        _enableTranslation = false;
        _enableSpeedAcceleration = false;

        NavMeshHit hit;
        NavMesh.SamplePosition(new Vector3(_body.position.x, -10.0F, _body.position.z), out hit, 100.0f, NavMesh.AllAreas);
        _body.position = hit.position;

        _body.GetComponent<NavMeshAgent>().enabled = true;
    }

    private void SetFreeflyMode()
    {
        _body.GetComponent<NavMeshAgent>().enabled = false;
        _freefly = true;
        _enableTranslation = true;
        _enableSpeedAcceleration = true;
    }

    // Apply requested cursor state
    private void SetCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = _wantedMode = CursorLockMode.None;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _wantedMode = CursorLockMode.Locked;
        }

        // Apply cursor state
        Cursor.lockState = _wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != _wantedMode);
    }

    private void CalculateCurrentIncrease(bool moving)
    {
        _currentIncrease = Time.deltaTime;

        if (!_enableSpeedAcceleration || _enableSpeedAcceleration && !moving)
        {
            _currentIncreaseMem = 0;
            return;
        }

        _currentIncreaseMem += Time.deltaTime * (_speedAccelerationFactor - 1);
        _currentIncrease = Time.deltaTime + Mathf.Pow(_currentIncreaseMem, 3) * Time.deltaTime;
    }

    private void Update()
    {
        if (!_active)
            return;

        SetCursorState();

        if (Cursor.visible)
            return;

        // Mode
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_freefly)
                SetConstrainMode();
            else
                SetFreeflyMode();
        }

        // Translation
        if (_enableTranslation)
        {
            transform.Translate(Vector3.forward * Input.mouseScrollDelta.y * Time.deltaTime * _translationSpeed);
        }

        // Movement
        if (_enableMovement)
        {
            Vector3 deltaPosition = Vector3.zero;
            float currentSpeed = _movementSpeed;

            if (Input.GetKey(_boostSpeed))
                currentSpeed = _boostedSpeed;

            if (Input.GetKey(KeyCode.Z))
                deltaPosition += _orientation.forward;

            if (Input.GetKey(KeyCode.S))
                deltaPosition -= _orientation.forward;

            if (Input.GetKey(KeyCode.Q))
                deltaPosition -= _orientation.right;

            if (Input.GetKey(KeyCode.D))
                deltaPosition += _orientation.right;

            if (_freefly && Input.GetKey(_moveUp))
                deltaPosition += _body.up;

            if (_freefly && Input.GetKey(_moveDown))
                deltaPosition -= _body.up;

            // Calc acceleration
            CalculateCurrentIncrease(deltaPosition != Vector3.zero);

            _body.position += deltaPosition * currentSpeed * _currentIncrease;
        }

        // Rotation
        if (_enableRotation)
        {
            // Pitch
            transform.rotation *= Quaternion.AngleAxis(
                -Input.GetAxis("Mouse Y") * _mouseSense,
                Vector3.right
            );

            // Paw
            transform.rotation = Quaternion.Euler(
                transform.eulerAngles.x,
                transform.eulerAngles.y + Input.GetAxis("Mouse X") * _mouseSense,
                transform.eulerAngles.z
            );

            _orientation.rotation = transform.rotation * Quaternion.Euler(
                -transform.eulerAngles.x,
                0,
                0
            );
        }

        // Return to init position
        if (Input.GetKeyDown(_initPositonButton))
        {
            _body.position = _initPosition;
            transform.eulerAngles = _initRotation;
            SetConstrainMode();
        }
    }
}
