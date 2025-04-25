using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour, IItemController
{
    private GameObject _itemInHands;
    private GameObject _itemPrefab;
    private GameObject _player;

    [SerializeField] private float rotationSpeed = 10f; // Скорость вращения
    [SerializeField] private float followSpeed = 5f; // Скорость следования за родителем
    [SerializeField] private float offsetDistance = 1f; // Дистанция от родителя
    [SerializeField] private bool _IsTaken;
    [SerializeField] private bool IsRotatable;

    private SpriteRenderer _spriteRenderer;
    //private Rigidbody2D _rigidbody;
    private Animator _animator;
    private CapsuleCollider2D _collider;
    private IActivateItem _iActivateItem;
    private bool _isActivated = false;

    void Update()
    {
        if (!_IsTaken)
        {
            return;
        }

        Rotate();
    }

    public void Initialize(GameObject player)
    {
        _player = player;
        _IsTaken = false;
        _animator = GetComponent<Animator>();
        //_rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _collider.isTrigger = false;
        _iActivateItem = GetComponent<IActivateItem>();

    }

    public void TakeItem()
    {
        _IsTaken = true;
        _animator.enabled = true;
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
    }

    public void RemoveItem()
    {
        _IsTaken = false;
        _animator.enabled = false;
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        if (_iActivateItem != null)
        {
            DeactivateItem();
        }
    }

    public void ActivateItem()
    {
        if (!_isActivated)
        {
            _iActivateItem.Activate();
            _isActivated = true;
        }
        else
        {
            _iActivateItem.Deactivate();
            _isActivated = false;
        }
    }

    public void DeactivateItem()
    {
        _iActivateItem.Deactivate();
        _isActivated = false;
    }

    private void Rotate()
    {
        // Получаем позицию курсора
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Вычисляем направление
        Vector3 direction = (mousePosition - transform.parent.position).normalized;

        if (IsRotatable)
        {
            // Вычисляем угол поворота
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);

            // Применяем вращение
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        // Вычисляем целевую позицию (на расстоянии offsetDistance от родителя)
        Vector3 targetPosition = transform.parent.position + direction * offsetDistance;

        // Плавное перемещение
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
