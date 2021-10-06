using UnityEngine.UI;
using UnityEngine;

public class Health_Heart : MonoBehaviour
{
    [SerializeField] Image heartFill;

    bool empty, full= true;

    public void Empty()
    {
        if (empty) {return;}
        empty = true;

        heartFill.fillAmount = 0;
    }

    public void Fill()
    {
        if (full) {return;}
        full = true;

        heartFill.fillAmount = 1;
    }
}
