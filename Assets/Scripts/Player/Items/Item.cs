using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] GameObject thisPrefab;
    [SerializeField] Sprite thisImage;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerBase>().AddToInventory(thisPrefab, thisImage);
            Destroy(gameObject);
        }
    }
}
