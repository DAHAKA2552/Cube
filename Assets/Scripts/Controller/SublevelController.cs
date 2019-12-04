using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class SublevelController : MonoBehaviour
{
    public event Action OnSublevelsComplete;
    public event TweenCallback OnShowComplete;
    public event TweenCallback OnHideComplete;


    #region Fields

    [SerializeField] PlayerController playerPrefab;
    [SerializeField] AnimationCurve ascensionCurve;
    [SerializeField] AnimationCurve descentionCurve;
    [SerializeField] float yOffset = -30f;
    [SerializeField] float delayBetweenRows = 0.2f;

    List<Transform> transforms = new List<Transform>();
    List<Stack<Transform>> stacks = new List<Stack<Transform>>();

    PlayerController player;
    GameObject currentSublevel;
    Sequence sequence;

    int currentSublevelIndex = 0;

    #endregion



    #region Properties

    public GameObject[] Sublevels { get; set; }

    #endregion



    #region Unity Lifecycle

    void OnEnable()
    {
        OnShowComplete += StartScripts;
        OnShowComplete += (() => InputManager.Instance.IsEnabled = true);
        OnShowComplete += SetCamera;

        //
        OnHideComplete += NextSublevel;

        Finish.OnFinish += StopScripts;
        Finish.OnFinish += HideCurrent;
        Finish.OnFinish += (() => InputManager.Instance.IsEnabled = false);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            StopScripts();
            HideCurrent();
            InputManager.Instance.IsEnabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            StopScripts();
            HideCurrent();
            InputManager.Instance.IsEnabled = false;
        }
    }


    void OnDisable()
    {
        OnShowComplete -= StartScripts;
        OnShowComplete -= (() => InputManager.Instance.IsEnabled = true);
        OnShowComplete -= SetCamera;

        //
        OnHideComplete -= NextSublevel;

        Finish.OnFinish -= StopScripts;
        Finish.OnFinish -= HideCurrent;
        Finish.OnFinish -= (() => InputManager.Instance.IsEnabled = false);
    } 

    #endregion



    #region Public Methods

    public void ShowFirst()
    {
        currentSublevelIndex = 0;

        ShowCurrent();
    }

    #endregion



    #region Private Methods

    void RestartCurrent()
    {

    }


    void RestartFirst()
    {
        ShowFirst();
    }


    void ShowCurrent()
    {
        if (currentSublevel != null)
        {
            Destroy(currentSublevel);

            player.ResetPlayer(transform.rotation);
        }
        else
        {
            player = Instantiate(playerPrefab, transform);
        }

        sequence = DOTween.Sequence();

        currentSublevel = Instantiate(Sublevels[currentSublevelIndex], transform);
        
        currentSublevel.transform.position = Move(currentSublevel.transform.position, yOffset);
        player.transform.position = Move(player.transform.position, -yOffset);

        currentSublevel.GetComponentsInChildren(transforms);

        //RemoveCoins(transforms);

        stacks = GroupByX(transforms);

        CreateSequence(-2 * yOffset, yOffset , ascensionCurve);

        if (OnShowComplete != null)
        {
            sequence.OnComplete(OnShowComplete);
        }

        UIManager.Instance.SetSublevelTo(currentSublevelIndex);
    }


    void HideCurrent()
    {
        sequence = DOTween.Sequence();

        CreateSequence(0, -yOffset, descentionCurve);

        if (OnHideComplete != null)
        {
            sequence.OnComplete(OnHideComplete);
        }
    }


    void NextSublevel()
    {
        currentSublevelIndex++;

        if (currentSublevelIndex >= Sublevels.Length)
        {
            OnSublevelsComplete?.Invoke();
        }
        else
        {
            CoinManager.Instance.NextSublevel();
            ShowCurrent();
        }
    }


    void CreateSequence(float y, float playerY, AnimationCurve curve)
    {
        int n = 1;
        bool isFirstS = true;

        foreach (Stack<Transform> stack in stacks)
        {
            Sequence s = DOTween.Sequence();
            bool isFirstT = true;

            s.AppendInterval(delayBetweenRows * n);

            foreach (Transform t in stack)
            {
                if (t != null)
                {
                    if (isFirstT)
                    {
                        s.Append(t.DOLocalMoveY(t.position.y + y, 1).SetEase(curve));
                        isFirstT = false;
                    }
                    else
                    {
                        s.Join(t.DOLocalMoveY(t.position.y + y, 1).SetEase(curve));
                    }
                }
            }

            if (isFirstS)
            {
                sequence.Append(s);
                isFirstS = false;

                sequence.Join(player.transform.DOLocalMoveY(player.transform.position.y + playerY, 1).SetEase(curve));
            }
            else
            {
                sequence.Join(s);
            }

            n++;
        }
    }


    void RemoveCoins(List<Transform> transforms)
    {
        List<Coin> allCoins = new List<Coin>();
        currentSublevel.GetComponentsInChildren<Coin>(allCoins);

        List<Transform> collectedCoins = CoinManager.Instance.GetCoins();

        if (collectedCoins != null)
        {
            if (allCoins != null)
            {
                foreach (Coin c in allCoins)
                {
                    foreach (Transform t in collectedCoins)
                    {
                        if ((c.transform.position.x == t.position.x) && (c.transform.position.z == t.position.z))
                        {
                            transforms.Remove(c.transform);
                            c.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }


    void StartScripts()
    {
        List<Levitation> levitations = new List<Levitation>();
        GetComponentsInChildren<Levitation>(levitations);

        foreach (Levitation l in levitations)
        {
            l.enabled = true;
        }

        List<Patrol> enemies = new List<Patrol>();
        GetComponentsInChildren<Patrol>(enemies);

        foreach (Patrol p in enemies)
        {
            p.enabled = true;
        }

        List<Gate> gates = new List<Gate>();
        GetComponentsInChildren<Gate>(gates);

        foreach (Gate g in gates)
        {
            g.enabled = true;
        }
    }


    void StopScripts()
    {
        List<Levitation> levitations = new List<Levitation>();
        GetComponentsInChildren<Levitation>(levitations);

        foreach (Levitation l in levitations)
        {
            l.enabled = false;
        }

        List<Patrol> enemies = new List<Patrol>();
        GetComponentsInChildren<Patrol>(enemies);

        foreach (Patrol p in enemies)
        {
            p.enabled = false;
        }

        List<Gate> gates = new List<Gate>();
        GetComponentsInChildren<Gate>(gates);

        foreach (Gate g in gates)
        {
            g.enabled = false;
        }
    }


    Vector3 Move(Vector3 pos, float y)
    {
        Vector3 temp = pos;
        temp = new Vector3(temp.x, temp.y + y, temp.z);

        return temp;
    }


    List<Stack<Transform>> GroupByX(List<Transform> transforms)
    {
        stacks = new List<Stack<Transform>>();

        transforms.RemoveAt(0);

        for (int i = 0; i < transforms.Count; i++)
        {
            if (transforms[i].childCount != 0) //Only if transforms[i] is "Gates"
            {
                //int delete = transforms[i].childCount;

                //for (int j = i + 1; j < i + delete + 1; j++)
                //{
                //    if (transforms[j].childCount != 0)
                //    {
                //        transforms.RemoveAt(i + 4);
                //    }
                //    transforms.RemoveAt(i + 1);
                //}
                for (int j = 0; j < 4; j++)
                {
                    transforms.RemoveAt(i + 1);
                }
            }
        }

        foreach (Transform t in transforms)
        {
            bool isFound = false;

            if (stacks.Count != 0)
            {
                foreach (Stack<Transform> stack in stacks)
                {
                    if (stack.Count != 0)
                    {
                        if (t.position.x == stack.Peek().position.x)
                        {
                            stack.Push(t);
                            isFound = true;
                            break;
                        }
                    }
                }
            }

            if (!isFound)
            {
                stacks.Add(new Stack<Transform>());

                foreach (Stack<Transform> stack in stacks)
                {
                    if (stack.Count == 0)
                    {
                        stack.Push(t);
                        break;
                    }
                }
            }
        }

        Sort(stacks);

        return stacks;
    }


    void SetCamera()
    {
        CinemachineVirtualCamera virtualCamera =
                LevelManager.Instance.mainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
        virtualCamera.Follow = player.CameraTarget;
    }


    void Sort(List<Stack<Transform>> stacks)
    {
        Stack<Transform> temp;
        int changes = 1;

        while (changes > 0)
        {
            changes = 0;

            for (int i = 0; i < stacks.Count - 1; i++)
            {
                if (stacks[i].Peek().position.x > stacks[i + 1].Peek().position.x)
                {
                    temp = stacks[i];
                    stacks[i] = stacks[i + 1];
                    stacks[i + 1] = temp;

                    changes++;
                }
            }
        }
    }

    #endregion
}
