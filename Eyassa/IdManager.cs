using Random = UnityEngine.Random;

namespace Eyassa;

public class IdManager
{
    internal static List<int> UsedNumbers = new();
    
    internal int GetNextId()
    {
        int randomNumber = Random.Range(0, int.MaxValue);

        while (UsedNumbers.Contains(randomNumber))
            randomNumber = Random.Range(0, int.MaxValue);
        UsedNumbers.Add(randomNumber);
        return randomNumber;
    }
    
}