using UnityEngine;

public class Gate_Laser : MonoBehaviour
{
    [SerializeField] int size;   //размер форот
    [SerializeField] float timeLaserOn; //время работы лазера
    [SerializeField] float timeLaserOff;//время на проход через воротами

    [SerializeField] GameObject laser; //лазер
    [SerializeField] GameObject box;   //конечный блок для лазера

    bool onLaserOn = true; //включаем лазер
    float timer = 0.0f;    //включаем таймер

    private void Start()
    {
        ++size; //увеличиваем размер, на 1, так как лазер идес с центра одного блока до центра другого
                //(скорее всего придется увеличивать в зависимости от размера блоков, которые могут изменяться в зависимости от размера экрана)
        if(transform.rotation.y == 0) //если ворота не повернуты
        {
            box.transform.position = new Vector3(transform.position.x + size, transform.position.y, transform.position.z); //устанавливает второй блок в соответствии с размером ворот
            laser.transform.localScale = new Vector3(size, laser.transform.localScale.y, laser.transform.localScale.z);    //увеличиваем длинну лазера
            laser.transform.position = new Vector3(transform.position.x + size / 2.0f, transform.position.y, transform.position.z); // ставим лазеп между воротами
        }
        else//по умолчанию поворот на 90 по Y
        {
            box.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - size); //устанавливает второй блок в соответствии с размером ворот
            laser.transform.localScale = new Vector3(size, laser.transform.localScale.y, laser.transform.localScale.z);    //увеличиваем длинну лазера
            laser.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - size / 2.0f); // ставим лазеп между воротами
        }

    }

    private void Update()
    {
        timer += Time.deltaTime; //увеличиваем таймер
        if(onLaserOn) //если ворота включены
        {
            if(timer >= timeLaserOn) //и таймер больше/равен времени работы ворот
            {
                timer = 0.0f;       //обнуляем таймер
                SwitchLaser();      //вызываем функцию ререключния ворот
                onLaserOn = false;  //выключаем лазер
            }
        }
        else // если ворота выключены
        {
            if (timer >= timeLaserOff) //и таймер больше/равен времени на проход через ворота 
            {
                timer = 0.0f;       //обнуляем таймер
                SwitchLaser();      //вызываем функцию ререключния ворот
                onLaserOn = true;  //включаем лазер
            }
        }
    }
    void SwitchLaser() //переключение лазера
    {
        if (onLaserOn) //и лазер включен
        {
            laser.transform.localScale = new Vector3(0, laser.transform.localScale.y, laser.transform.localScale.z); //уменьшаем длинну лазера до 0
            laser.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); // сменяем позицию в центр первого блока
        }
        else // если лазер выключен
        {
            laser.transform.localScale = new Vector3(size, laser.transform.localScale.y, laser.transform.localScale.z); //увеличиваем длинну лазера
            if (transform.rotation.y == 0) //если пращение нету
            {
                laser.transform.position = new Vector3(transform.position.x + size / 2.0f, transform.position.y, transform.position.z); //ставим лазер между блоками
            }
            else//если было вращение (по умолчанию на 90 по Y)
            {
                laser.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - size / 2.0f); //ставим лазер между блоками
            }
        }
    }
}
