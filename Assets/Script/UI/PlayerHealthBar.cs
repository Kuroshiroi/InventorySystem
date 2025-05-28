using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    [field: SerializeField]
    public Image HealthFillImage;

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
        HealthFillImage.fillAmount = (float)playerStats.currentHealth / playerStats.maxHealth;
        tmp_text.text = "" + playerStats.currentHealth;
    }
}
