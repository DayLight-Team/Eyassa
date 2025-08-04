using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Eyassa;

internal class IdManager
{
    internal static List<int> UsedNumbers = new();
    
    internal static int GetNextId()
    {
        int randomNumber = Random.Range(0, int.MaxValue);

        while (UsedNumbers.Contains(randomNumber))
            randomNumber = Random.Range(0, int.MaxValue);
        UsedNumbers.Add(randomNumber);
        return randomNumber;
    }
    
}