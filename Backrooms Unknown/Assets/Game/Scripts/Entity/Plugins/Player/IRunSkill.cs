using UnityEngine;

public interface IRunSkill 
{
    void Initialize(IStaminaController iStaminaController, IAnimatorController animatorController, ISoundController soundController, Rigidbody2D rigidbody, float speed);

    void TryToRun(Vector2 direction);

    void TryToSlowDown();
}
