using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField] Image thisItem;
    void Update()
    {
        if (thisItem.sprite == null) thisItem.color = new Color32(255, 255, 225, 0);
        else thisItem.color = new Color32(255, 255, 225, 255);
    }
}
