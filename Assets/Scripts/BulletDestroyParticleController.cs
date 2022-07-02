using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyParticleController : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
