using UnityEngine;

public interface IStaminaController
{
    float GetStaminaValue();

    bool TrySpendStamina(float stamina);

    float StaminaRegenerationRest();
}
