using UnityEngine;

public interface ISoundController
{
    void MakeStepSound(Vector2 direction);

    void MakeHitSound();

    void SpeedUp(float speedUp);

    void SlowDown();
}
