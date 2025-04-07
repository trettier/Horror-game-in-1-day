using UnityEngine;

public class RunSkill : MonoBehaviour, IRunSkill
{
    private IStaminaController _iStaminaController;
    private IAnimatorController _animatorController;
    private ISoundController _soundController;
    private Rigidbody2D _rigidbody;

    private float _speed;
    [SerializeField] private float _speedCoefficent = 0.2f;
    [SerializeField] private float _staminaCost = 0.1f;
    [SerializeField] private float _animatorSpeedUpCoefficent = 1.5f;
    [SerializeField] private float _SoundSpeedUpCoefficent = 1.4f;


    public void Initialize(IStaminaController iStaminaController, IAnimatorController animatorController, ISoundController soundController, Rigidbody2D rigidbody, float speed)
    {
        _iStaminaController = iStaminaController;
        _animatorController = animatorController;
        _soundController = soundController;
        _rigidbody = rigidbody;
        _speed = speed;
    }
    public void TryToRun(Vector2 direction)
    {
        if (_iStaminaController.TrySpendStamina(_staminaCost)) 
        { 
            _rigidbody.AddForce(direction * _speed * _speedCoefficent);
            _animatorController.SpeedUp(_animatorSpeedUpCoefficent);
            _soundController.SpeedUp(_SoundSpeedUpCoefficent);
        }
        else
        {
            TryToSlowDown();
        }
    }

    public void TryToSlowDown()
    {
        _animatorController.SlowDown();
        _soundController.SlowDown();
    }
}
