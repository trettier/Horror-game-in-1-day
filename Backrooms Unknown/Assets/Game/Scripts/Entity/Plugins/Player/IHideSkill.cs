using UnityEngine;

public interface IHideSkill
{

    void Initialize(SpriteRenderer spriteRenderer, CapsuleCollider2D capsuleCollider2D, Rigidbody2D rigidbody);

    void TryToHideOrExit();

    bool IsHidden();

    void IsObjectToHideNear(bool isObjectToHideNear);

    void SetObjectToHidePosition(Vector2 objectToHidePosition);
}
