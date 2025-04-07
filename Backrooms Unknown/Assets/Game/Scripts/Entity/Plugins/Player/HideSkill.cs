using UnityEngine;

public class HideSkill : MonoBehaviour, IHideSkill
{
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;
    [SerializeField] private GameObject _lighter;
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _playerCollider2D;
    protected Rigidbody2D _rigidbody;

    private bool _isObjectToHideNear = false;
    private bool _isOwnerInObjectToHide = false;

    private Vector2 _objectToHidePosition;
    private Vector2 _playerPositionBeforeHide;


    public void Initialize(SpriteRenderer spriteRenderer, CapsuleCollider2D capsuleCollider2D, Rigidbody2D rigidbody)
    {
        _spriteRenderer = spriteRenderer;
        _playerCollider2D = capsuleCollider2D;
        _rigidbody = rigidbody;
    }

    public void TryToHideOrExit()
    {
        if (_isObjectToHideNear)
        {
            EnterTheBox();
        }
        else if (_isOwnerInObjectToHide)
        {
            ExitTheBox();
        }

    }

    private void EnterTheBox()
    {
        _rigidbody.linearVelocity = Vector2.zero;
        _spriteRenderer.enabled = false;
        _shadowSpriteRenderer.enabled = false;
        _playerCollider2D.enabled = false;
        _lighter.SetActive(false);

        gameObject.tag = "PlayerHidden";

        _playerPositionBeforeHide = transform.position;
        transform.position = _objectToHidePosition;

        _isOwnerInObjectToHide = true;
    }
    
    private void ExitTheBox()
    {
        _spriteRenderer.enabled = true;
        _shadowSpriteRenderer.enabled = true;
        _playerCollider2D.enabled = true;
        _lighter.SetActive(true);

        gameObject.tag = "Player";
        transform.position = _playerPositionBeforeHide;

        _isOwnerInObjectToHide = false;
    }

    public bool IsHidden()
    {
        return _isOwnerInObjectToHide;
    }

    public void IsObjectToHideNear(bool isObjectToHideNear)
    {
        _isObjectToHideNear = isObjectToHideNear;
    }

    public void SetObjectToHidePosition(Vector2 objectToHidePosition)
    {
        _objectToHidePosition = objectToHidePosition;
    }
}
