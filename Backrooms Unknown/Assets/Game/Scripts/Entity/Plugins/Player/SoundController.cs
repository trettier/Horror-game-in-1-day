using UnityEngine;

public class SoundController : MonoBehaviour, ISoundController
{
    [SerializeField] private AudioSource _stepSound;
    [SerializeField] private AudioSource _onHitSound;

    private bool _isMoving = false;

    public void MakeStepSound(Vector2 direction)
    {
        bool wasMoving = _isMoving;
        _isMoving = (direction.x != 0 || direction.y != 0);

        if (_isMoving)
        {
            if (!wasMoving)
            {
                _stepSound.Play();
            }
        }
        else if (wasMoving)
        {
            _stepSound.Stop();
        }
    }

    public void MakeHitSound()
    {
        _onHitSound.Play();
    }

    public void SpeedUp(float speedUp)
    {
        _stepSound.pitch = speedUp;
    }
    public void SlowDown()
    {
        _stepSound.pitch = 1.2f;
    }

}
