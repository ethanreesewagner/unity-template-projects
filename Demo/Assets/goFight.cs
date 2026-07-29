using UnityEngine;
using UnityEngine.SceneManagement;

public class goFight : MonoBehaviour
{
    [SerializeField] private string sceneName = "FightingScene";

    private void OnMouseDown()
    {
        SceneManager.LoadScene(sceneName);
    }
}
