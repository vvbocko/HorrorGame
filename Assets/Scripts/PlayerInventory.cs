using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    
    [SerializeField] private List<ItemDefinition> itemDefinitions = new List<ItemDefinition>();

    public void AddItem(ItemDefinition item)
    {
        itemDefinitions.Add(item);
    }
    public void RemoveItem(ItemDefinition item) 
    { 
        itemDefinitions.Remove(item); 
    }
    public bool HasItem(ItemDefinition item)
    {
        return itemDefinitions.Contains(item);
    }
    public bool HasItem(int itemID)
    {
        for(int i = 0; i < itemDefinitions.Count; i++)
        {
            if(itemDefinitions[i].itemID == itemID)
            {
                return true;
            }
        }
        return false;
    }

}
