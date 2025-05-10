using BallDrop;
using BallDrop.Audio;
using UnityEngine;

public class CoinBase : MonoBehaviour
{
    Vector3 initialScale;
    [SerializeField]
    AudioClip CoinCollectionSound;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    public void Activate(Vector3 position, Quaternion rotation, Transform parent)
    {
        gameObject.SetActive(true);
        gameObject.transform.SetPositionAndRotation(position, rotation);
        gameObject.transform.localScale = initialScale;
        gameObject.transform.SetParent(parent);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MyEventManager.Instance.UpdateCoins.Dispatch();
            if(CoinCollectionSound != null)
                AudioManager.Instance.PlayEffect(CoinCollectionSound);
            GameObject particles = ObjectPool.Instance.GetCoinCollectionParticles();
            particles.transform.position = transform.position;
            particles.GetComponent<ParticleSystem>().Play();
            Deactivate();
        }
    }

    public bool ActiveInHierarchy
    {
        get { return gameObject.activeInHierarchy; }
    }

    public void Deactivate()
    {
        transform.parent = ObjectPool.Instance.PooledObjectsHolder;
        gameObject.SetActive(false);
    }

}
