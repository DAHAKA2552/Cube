using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] float timeLaserOn = 1f;
    [SerializeField] float timeLaserOff = 2f;
    [SerializeField] float timeBeforeEnabling = 1f;
    [SerializeField] GameObject laser;

    float timer;
    float timeLimit;
    bool isLaserOn = false;


    void Start()
    {
        timer = 0.0f;
        timeLimit = timeBeforeEnabling;

        laser.SetActive(false);
    }


    void Update()
    {
        if (timer > timeLimit)
        {
            if (isLaserOn)
            {
                timeLimit = timeLaserOff;
            }
            else
            {
                timeLimit = timeLaserOn;
            }

            laser.SetActive(!isLaserOn);
            isLaserOn = !isLaserOn;
            timer = 0.0f;
        }

        timer += Time.deltaTime;
    }
}
