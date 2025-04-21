

public interface IInventoryManager
{

    bool AddItem(Item item);

    Item GetSelectedItem();

    void UseSelectedItem();

    void ChangeSelectedSlot(int newValue);

}