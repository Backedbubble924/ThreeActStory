using UnityEngine;

public class Damage : MonoBehaviour
{

    private PHealth pHealth;
    public int damage = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        pHealth = FindObjectOfType<PHealth>();
        if (collision.gameObject.tag == "Player")
        {
            pHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
