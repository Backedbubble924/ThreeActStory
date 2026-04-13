using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PHealth : MonoBehaviour
{

    public int health;
    public int maxHealth = 10;
    public Image Health;
    public string Scene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(Scene);
        }
    }
}
