using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endGame : MonoBehaviour
{
    
    public void OnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnPlayAgain()
    {
        SceneManager.LoadScene(1);
    }


}
