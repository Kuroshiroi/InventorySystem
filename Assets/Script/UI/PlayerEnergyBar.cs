using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyBar : MonoBehaviour
{
    [field: SerializeField]
    public Image EnergyFillImage;

    [field: SerializeField]
    public Text tmp_text;

    [field: SerializeField]
    PlayerStatController playerStats;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnergyFillImage.fillAmount = (float)playerStats.currentEnergy / playerStats.maxEnergy;
        tmp_text.text = "" + playerStats.currentEnergy;
    }
}
