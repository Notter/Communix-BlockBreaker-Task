using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {
        Destroy(gameObject, 2f);
    }
}
