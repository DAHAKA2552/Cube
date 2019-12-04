using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float rotationSpeed;


    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            CoinManager.Instance.AddCoin(this);

            AudioManager.Instance.PlaySFXClip(ClipName.CoinPartCollected);

            gameObject.SetActive(false);
            
        }
    }


    void Update()
    {
        transform.Rotate(Vector3.right, Time.deltaTime * rotationSpeed);
    }



}
