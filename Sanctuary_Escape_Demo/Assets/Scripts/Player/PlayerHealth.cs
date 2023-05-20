using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100f;

    public Image frontHealthBar;
    public Image backHealthBar;

    private float lerpTimer;
    public float chipSpeed = 2f;

    private PlayerMotor playerMotor;
    public GameObject grayScreen;
    public MouseScript mouseScript;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playerMotor = GetComponent<PlayerMotor>();
        mouseScript = GetComponent<MouseScript>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = currentHealth / maxHealth;

        if (fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if (fillFront < healthFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = healthFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        lerpTimer = 0f;
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        lerpTimer = 0f;
    }

    private void Die()
    {
        playerMotor.enabled = false;
        mouseScript.enabled = false;
        //grayScreen.SetActive(true);

    }

}
