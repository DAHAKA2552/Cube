using System.Collections;
using UnityEngine;

public class DestroiPlatform : MonoBehaviour
{
    [SerializeField] float timer; //время, после которого проверяется, ушел ли игрок с блока
    bool isPlayerOnBlock = false; //встал ли игрок на блок 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //если игрок коснулся блока
        {
            if (other.transform.position.x == gameObject.transform.position.x) // если позиция игрока находится над позицией блока
            {
                isPlayerOnBlock = true; //игрок встал на блок
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlayerOnBlock == true) //если игрок до этого втсавал на блок
        {
            StartCoroutine(CheckPlatform()); //заупскаем что бы проверить ушел ли игрок с блока
        }
    }

    private IEnumerator CheckPlatform()
    {
        yield return new WaitForSeconds(timer); // через время(0.1 по дефолту)
        if (!Physics.Raycast(transform.position, Vector3.up, 1)) // если блока над платформой нету
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true; //включаем гравитацию
            gameObject.GetComponent<Collider>().enabled = false; //отключаем колайдер, что бы игрок не смог повторно встать на блок
        }
    }
}
