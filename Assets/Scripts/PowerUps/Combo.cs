using UnityEngine;

public class Combo : MonoBehaviour
{
    [SerializeField] private int comboInRowCounter;
    [SerializeField] private int comboInRowNeededForBonus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int OnChargingCombo()
    {
        comboInRowCounter++;
        if (comboInRowCounter == comboInRowNeededForBonus)
        {
            comboInRowCounter = 0;
            return 1; //someshit;
        }
        else
        {
            return 0;
        }
    }
}
