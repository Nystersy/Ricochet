using UnityEngine;

public class HitEffect : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.2f);
    }
}