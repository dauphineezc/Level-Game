using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayLevel1(){
        SceneManager.LoadSceneAsync("Scenes/lvl1");
    }

    public void PlayLevel2(){
        SceneManager.LoadSceneAsync("lvl2");
    }
}
