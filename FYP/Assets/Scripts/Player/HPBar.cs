using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    GameObject player;
    PlayerHealth playerHealth;

    public static HPBar instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.Player);
        playerHealth = player.GetComponent<PlayerHealth>();
        healthBar.maxValue = playerHealth.getMaxHP();
        healthBar.value = playerHealth.getPlayerHP();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (playerHealth.GetHealthStatus() == PlayerHealth.healthStatus.Fine)
        {
            Color color = new Color(51f / 255f, 153f / 255f, 255f / 255f);
            healthBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;
        }
        else if (playerHealth.GetHealthStatus() == PlayerHealth.healthStatus.Caution)
        {
            Color color = new Color(233f / 255f, 128f / 255f, 0f / 255f);
            healthBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;
        }
        else if (playerHealth.GetHealthStatus() == PlayerHealth.healthStatus.Danger)
        {
            Color color = new Color(255f / 255f, 0f / 255f, 0f / 255f);
            healthBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;
        }
    }

    public void updateHealthBar(float healthRemaining)
    {
        healthBar.value = healthRemaining;
    }
}