using System.Linq;

var arr = new int[] { 1, 2, 2, 3, 1 };
Console.WriteLine("Array: [" + String.Join(",", arr) + $"] {Solution.FindShortestSubArray(arr)}");
arr = [1, 2, 2, 3, 1, 4, 2];
Console.WriteLine("Array: [" + String.Join(",", arr) + $"] {Solution.FindShortestSubArray(arr)}");
arr = [5];
Console.WriteLine("Array: [" + String.Join(",", arr) + $"] {Solution.FindShortestSubArray(arr)}");
arr = [5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5];
Console.WriteLine("Array: [" + String.Join(",", arr) + $"] {Solution.FindShortestSubArray(arr)}");
arr = [5, 5, 5, 5, 5, 3, 5, 5, 5, 5, 5, 5];
Console.WriteLine("Array: [" + String.Join(",", arr) + $"] {Solution.FindShortestSubArray(arr)}");
arr = [5, 5, 5, 5, 5, 3, 5, 5, 5, 5, 0, 49999, 49999];
Console.WriteLine("Array: [" + String.Join(",", arr) + $"] {Solution.FindShortestSubArray(arr)}");

public static class Solution
{
    public static int FindShortestSubArray(int[] nums)
    {
        var items = nums.ToList(); // Lists are easier to work with
        var reducedItems = items.Distinct().ToList();
        var dict = reducedItems.ToDictionary(i => i, i => items.Count(x => x == i)); // Find the degree of each item

        var maxCount = dict.Values.Max(); // This is the max degree of the array

        var minArrayLength = dict
            .Where(d => d.Value == maxCount) // Find the items with the max degree (there might be more than one item that has the max degree)
            .Select(d => GetSubArrayLength(items, d.Key)) // Get the length of the subarray for each item
            .Min(); // Get the smallest value

        return minArrayLength;
    }

    /// <summary>
    /// Helper method to get the length of the subarray
    /// </summary>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static int GetSubArrayLength(List<int> items, int item)
    {
        var sub = items.Skip(items.IndexOf(item)).ToList(); // this removes the items before the max item
        return sub.Take(sub.LastIndexOf(item) + 1).Count(); // this removes the items after the max item
    }

    /// <summary>
    /// This is the what I had written at the end of the interview
    /// It does not account for the case where there are multiple items with the same degree but different lengths
    /// </summary>
    /// <param name="nums"></param>
    /// <returns></returns>
    public static int FindShortestSubArray_Interview(int[] nums)
    {
        var items = nums.Distinct().ToList();
        var values = items.Select(i => nums.Count(x => x == i)).ToList();
        var max = values.Max();

        var index = values.IndexOf(max);
        var value = values[index];
        Console.WriteLine($"Value: {value}");

        var sub = nums.Skip(index).ToList();
        Console.WriteLine(String.Join(",", sub));
        Console.WriteLine(sub.LastIndexOf(value));
        sub = sub.Take(sub.LastIndexOf(value)).ToList();
        Console.WriteLine(String.Join(",", sub));


        return max;
    }


    public static int FindShortestSubArray_Orig(int[] nums)
    {
        Dictionary<int, int> frequency = new Dictionary<int, int>();
        foreach (int num in nums)
        {
            if (frequency.ContainsKey(num))
            {
                frequency[num] += 1;
            }
            else
            {
                frequency[num] = 1;
            }
        }
        int degree = 0;
        foreach (int count in frequency.Values)
        {
            degree = Math.Max(degree, count);
        }
        int minLength = int.MaxValue;
        for (int i = 0; i < nums.Length; i++)
        {
            Dictionary<int, int> subFreq = new Dictionary<int, int>();
            for (int j = i; j < nums.Length; j++)
            {
                if (subFreq.ContainsKey(nums[j]))
                {
                    subFreq[nums[j]] += 1;
                }
                else
                {
                    subFreq[nums[j]] = 1;
                }
                int maxSubFreq = 0;
                foreach (int count in subFreq.Values)
                {
                    maxSubFreq = Math.Max(maxSubFreq, count);
                }
                if (maxSubFreq == degree)
                {
                    minLength = Math.Min(minLength, j - i + 1);
                    break;
                }
            }
        }
        return minLength;
    }
}