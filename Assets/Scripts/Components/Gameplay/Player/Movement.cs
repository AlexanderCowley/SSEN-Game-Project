using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _linearVelocity = 10.0f;
    [SerializeField] private float _circularVelocity = 10.0f;
    private Vector3 _direction;
    private Vector3 _lookDirection;
    private Quaternion _lookRotation;
    private CharacterController _characterController;
    private Animator _animator;

    public Vector3 LookDirection { get { return _lookDirection; } }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (MovementCheck())
        {
            Move();
            Rotate();
        }
    }

    private void Move()
    {
        _characterController.Move(_direction * Time.deltaTime * _linearVelocity);
        _animator.SetBool("isMoving", _direction != Vector3.zero);
    }

    private void Rotate()
    {
        if (_lookDirection != Vector3.zero)
        {
            _lookRotation = Quaternion.LookRotation(_lookDirection.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, _circularVelocity * Time.deltaTime);
        }        
    }

    private bool MovementCheck()
    {
        AnimatorStateInfo asi = _animator.GetCurrentAnimatorStateInfo(0);
        bool actionAnimations = asi.IsName("WokTossing") || asi.IsName("Chopping") || asi.IsName("PouringOil") || asi.IsName("PouringSoySauce");
        if (actionAnimations)
        {
            return false;
        }
        return true;
    }

    public void Horizontal(float value)
    {
        _direction = new Vector3(value, _direction.y, _direction.z);
        if (_direction != Vector3.zero)
        {
            _lookDirection = new Vector3(value, _lookDirection.y, _lookDirection.z);
        }
    }

    public void Vertical(float value)
    {
        _direction = new Vector3(_direction.x, _direction.y, value);
        if (_direction != Vector3.zero)
        {
            _lookDirection = new Vector3(_lookDirection.x, _lookDirection.y, value);
        }
    }
}
