using UnityEngine;

public class Collectable : MonoBehaviour
{

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    private void OnTriggerEnter(Collider other)
    {
            if(other.CompareTag("Player"))
        {
            ScoreManager.Instance.IncreaseScore();          
            //gameObject.SetActive(false);
                Destroy(this.gameObject);
        }
    }
}
