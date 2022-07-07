using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public enum healthStatus
    {
        Fine,
        Caution,
        Danger,
    }

    //[SerializeField]
    //GameObject enemy;

    [SerializeField]
    float playerHealth;
    float maxHealth;
    [SerializeField]
    private healthStatus healthState;

    public HPBar hpBar;

    private float iframeDuration;
    private int flash;
    private SpriteRenderer spriteRenderer;

    public bool isInvulnarable;

    private void Awake()
    {
        setPlayerHealth(100);
        setMaxHp(100);
        checkTheState(); 
        isInvulnarable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        setPlayerHealth(100);
        setMaxHp(100);
        checkTheState();

        iframeDuration = 3f;
        flash = 10;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Physics2D.GetIgnoreLayerCollision(3, 7));
    }

    public void checkTheState()
    {
        if (getPlayerHP() <= getMaxHP() && getPlayerHP() >= 70)
        {
            setHealthStatus(healthStatus.Fine);
        }
        else if (getPlayerHP() < 70 && getPlayerHP() >= 40)
        {
            setHealthStatus(healthStatus.Caution);
        }
        else if(getPlayerHP() < 40 && getPlayerHP() >= 1)
        {
            setHealthStatus(healthStatus.Danger);
        }
        else if (getPlayerHP() <= 0)
        {
            die();
        }
    }

    private IEnumerator Invulnarable()
    {
        Physics2D.IgnoreLayerCollision(7, 6);
        isInvulnarable = true;
        //Invulnarable time
        for (int i = 0; i < flash; i++)
        {
            spriteRenderer.color = new Color(0, 0, 0, 0.5f);
            yield return new WaitForSeconds(iframeDuration / (flash * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iframeDuration / (flash * 2));

            //invul(0f);
        }
        Physics2D.IgnoreLayerCollision(7, 6, false);
        isInvulnarable = false;
        //invul(25f);
    }

    public void takeDamage(float healthCut)
    {
        if (!isInvulnarable)
        {
            float healthRemaining = getPlayerHP() - healthCut;
            setPlayerHealth(healthRemaining);
            hpBar.updateHealthBar(healthRemaining);
            StartCoroutine(Invulnarable());
            checkTheState();
        }
        else
        {
            return;
        }
    }

    public void heal(float healthGain)
    {
        float healthRemaining = getPlayerHP() + healthGain;
        if(healthRemaining >= getMaxHP())
        {
            healthRemaining = getMaxHP();
        }
        setPlayerHealth(healthRemaining);
    }

    void resetHP()
    {
        setPlayerHealth(getMaxHP());
        checkTheState();
    }

    void die()
    {
        Debug.Log("You are dead");
        //resetHP();
        //Destroy(gameObject);
    }

    public float getPlayerHP()
    {
        return playerHealth;
    }
    public void setPlayerHealth(float hp)
    {
        playerHealth = hp;
        hpBar.updateHealthBar(hp);
        checkTheState();
    }
    public float getMaxHP()
    {
        return maxHealth;
    }
    public void setMaxHp(float hp)
    {
        maxHealth = hp;
    }

    public healthStatus GetHealthStatus()
    {
        return healthState;
    }

    public void setHealthStatus(healthStatus status)
    {
        healthState = status;
    }
}
