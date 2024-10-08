using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset inputActionAsset;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private InputAction _moveLeftAction;
    private InputAction _moveRightAction;
    private InputAction _jumpAction;

    private bool _RunningLeft;
    private bool _RunningRight;
    private bool _Jumping;
    public bool _isGrounded;

    private Rigidbody rb;
    private GameTimer _gameTimer;
    private Animator _animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        _gameTimer = FindAnyObjectByType<GameTimer>();
        _animator = GetComponentInChildren<Animator>();

        _moveLeftAction = inputActionAsset.FindActionMap("PlayerMovement").FindAction("Run Left");
        _moveRightAction = inputActionAsset.FindActionMap("PlayerMovement").FindAction("Run Right");
        _jumpAction = inputActionAsset.FindActionMap("PlayerMovement").FindAction("Jump");

        _moveLeftAction.Enable();
        _moveRightAction.Enable();
        _jumpAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        _RunningLeft = _moveLeftAction.ReadValue<float>() > 0;
        _RunningRight = _moveRightAction.ReadValue<float>() > 0;
        _Jumping = _jumpAction.ReadValue<float>() > 0;

        rotatePlayer();
    }

    void FixedUpdate()
    {
        if (_gameTimer.gameEnded)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            _animator.SetBool("isCheering", true);
            return;
        }

        MovePlayer();

        if (_Jumping && _isGrounded)
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        Vector3 velocity = rb.linearVelocity;

        if (_RunningLeft)
        {
            velocity.z = -moveSpeed;
            _animator.SetBool("isRunning", true);
        }
        else if (_RunningRight)
        {
            velocity.z = moveSpeed;
            _animator.SetBool("isRunning", true);
        }
        else
        {
            velocity.z = 0;
            _animator.SetBool("isRunning", false);
        }

        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, velocity.z);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void rotatePlayer()
    {
        if (_RunningLeft)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (_RunningRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }


    void OnDestroy()
    {
        _moveLeftAction.Disable();
        _moveRightAction.Disable();
        _jumpAction.Disable();
    }
}
