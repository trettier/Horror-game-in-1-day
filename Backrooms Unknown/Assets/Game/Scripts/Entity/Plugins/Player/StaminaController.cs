using UnityEngine;

public class StaminaController : MonoBehaviour, IStaminaController
{
    [SerializeField] private float _currentStamina = 100;
    [SerializeField] private float _maxStamina = 100;
    [SerializeField] private float _staminaRegenerationRest = 0;
    [SerializeField] private float _staminaSegenerationSpeed = 0.1f;
    
    private void FixedUpdate()
    {
        if (_staminaRegenerationRest > 0)
        {
            _staminaRegenerationRest -= Time.deltaTime;
        }
        else if (_currentStamina < _maxStamina)
        {
            _currentStamina += _staminaSegenerationSpeed;
        }
    }
    
    public float GetStaminaValue()
    {
        return _currentStamina / _maxStamina;
    }

    public bool TrySpendStamina(float stamina)
    {
        if (_currentStamina < stamina)
        {
            _staminaRegenerationRest = 0;
            return false;
        }
        else
        {
            _currentStamina -= stamina;
            _staminaRegenerationRest = 1.5f;
            return true;
        }
    }

    public float StaminaRegenerationRest()
    {
        return _staminaRegenerationRest;
    }
}
