using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;

public class SceneReferenceContainer : MonoBehaviour
{
    [SerializeField] protected SceneReference scene;

    public void Load(){
        SceneManager.LoadScene(scene.BuildIndex);
    }
}
