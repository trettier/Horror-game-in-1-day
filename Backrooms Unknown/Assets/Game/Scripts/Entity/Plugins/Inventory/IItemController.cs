

using UnityEngine;

public interface IItemController
{
    void Initialize(GameObject player);

    void TakeItem();

    void RemoveItem();

    void ActivateItem();

    void DeactivateItem();
}