using UnityEngine;

public interface IItemState
{
    void Enter();       // Вызывается при входе в состояние

    void Exit();        // Вызывается при выходе из состояния

    void Update();      // Обновление (если нужно)

    void OnPlayerEnterTrigger();

    void OnPlayerExitTrigger();

    void OnPlayerActivate();
}