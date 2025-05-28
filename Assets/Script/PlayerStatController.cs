using UnityEngine;

public class PlayerStatController : MonoBehaviour
{
    [field: SerializeField]
    public  int maxHealth { get; set; }

    [field :SerializeField]
    public int maxEnergy { get; set; }

    [field: SerializeField]
    public int currentHealth { get; set; }

    [field: SerializeField]
    public int currentEnergy { get; set; }

    void Awake()
    {
        currentHealth = maxHealth / 2;
        currentEnergy = maxEnergy / 2;

    }

    public void UpdateHealth (int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void UpdateEnergy(int amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }
}
