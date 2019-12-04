using System;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    public event Action<Vector3> OnSwipe;

    float dragDistance = Screen.width* 25 / 100;
    Vector2 startpoint = new Vector2(Screen.width / 2, Screen.height / 2);
    Vector2 timeposition;
    bool isMoving = false;


    public bool IsEnabled { get; set; }


    void Update()
    {
        if (IsEnabled)
        {
            KeyboardControls();
            SwipeControls();
        }
    }


    void KeyboardControls()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (OnSwipe != null)
            {
                OnSwipe(Vector3.forward);
            }

            UIManager.Instance.HideButtons();
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (OnSwipe != null)
            {
                OnSwipe(Vector3.back);
            }

            UIManager.Instance.HideButtons();
        }

        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (OnSwipe != null)
            {
                OnSwipe(Vector3.left);
            }

            UIManager.Instance.HideButtons();
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (OnSwipe != null)
            {
                OnSwipe(Vector3.right);
            }

            UIManager.Instance.HideButtons();
        }
    }


    void SwipeControls()
    {
        if (Input.touchCount > 0) //если было касание
        {
            Touch touch = Input.GetTouch(0); //получаем певое касание

            if (touch.phase == TouchPhase.Began)
            {
                startpoint = Input.GetTouch(0).position; //получаем точку касания
            }
            switch (touch.phase)
            {
                case TouchPhase.Began: //при касаниее
                    startpoint = Input.GetTouch(0).position; //получаем точку касания
                    break;


                case TouchPhase.Moved: //при движение
                    timeposition = Input.GetTouch(0).position; //получаем новую точку
                    if (Mathf.Abs(timeposition.x - startpoint.x) > dragDistance || Mathf.Abs(timeposition.y - startpoint.y) > dragDistance) // было ли движение
                    {
                        if (!isMoving)
                        {
                            Moving();
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    if (!isMoving)
                    {
                        Moving();
                    }
                    isMoving = false;
                    break;
            }
        }
    }

    void Moving()
    {
        if (Mathf.Abs(timeposition.x - startpoint.x) > Mathf.Abs(timeposition.y - startpoint.y)) //если оно было по оси Х (если движение по Х больше чем по У)
        {
            if (timeposition.x > startpoint.x)  //движение вправо
            {
                OnSwipe(Vector3.right);
                isMoving = true;

                UIManager.Instance.HideButtons();
            }
            else
            {
                OnSwipe(Vector3.left);
                isMoving = true;

                UIManager.Instance.HideButtons();
            }
        }
        else
        {
            if (timeposition.y > startpoint.y)
            {
                OnSwipe(Vector3.forward);
                isMoving = true;

                UIManager.Instance.HideButtons();
            }
            else
            {
                OnSwipe(Vector3.back);
                isMoving = true;

                UIManager.Instance.HideButtons();
            }
        }
    }
}
