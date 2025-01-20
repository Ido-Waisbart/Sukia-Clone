// Called before all scripts via project settings' script execution order..

using UnityEngine;

public class Seeder : MonoBehaviour
{
    [SerializeField] protected bool useSeed = false;
    [SerializeField] protected int seed = 0;

    void Start()
    {
        if(useSeed) UnityEngine.Random.InitState(seed);
    }
}
