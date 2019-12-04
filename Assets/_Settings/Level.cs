using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField] GameObject[] sublevels;
    [SerializeField] int totalAmountOfCoins;


    public GameObject GetSublevel(int index, out bool isLast)
    {
        isLast = false;

        if (index < sublevels.Length)
        {
            if (index == sublevels.Length - 1)
            {
                isLast = true;
            }

            return sublevels[index];
        }
        
        return null;
    }


    public GameObject[] GetAllSublevels()
    {
        return sublevels;
    }
}
