using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelend : MonoBehaviour
{
    public void OnCollisionEnter(Collision other)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }
}
