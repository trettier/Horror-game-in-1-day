using UnityEngine;

public interface IItemState
{
    void Enter();       // ���������� ��� ����� � ���������

    void Exit();        // ���������� ��� ������ �� ���������

    void Update();      // ���������� (���� �����)

    void OnPlayerEnterTrigger();

    void OnPlayerExitTrigger();

    void OnPlayerActivate();
}