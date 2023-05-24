namespace Advc.Utils
{
    class PatternUtil
    {
        public static Dictionary<List<T>,int> FindPatterns<T>(IReadOnlyCollection<T> input, int minLength = 2)
        {
            var listToString = (List<T> list) => $"[{string.Join(",", list)}]";

            Dictionary<string, (List<T>, int)> listMap = new();

            for (int length = input.Count() - 1; length >= minLength; length --)
            {
                for (int startIndex = 0; startIndex + length <= input.Count(); startIndex ++)
                {
                    var subList = input.Skip(startIndex).Take(length).ToList();
                    var key = listToString(subList);

                    if (!listMap.ContainsKey(key))
                    {
                        listMap.Add(key, (subList, 0));
                    }

                    listMap[key] = (listMap[key].Item1, listMap[key].Item2 + 1);
                }
            }

            return listMap.Where(p => p.Value.Item2 > 1).ToDictionary(d => d.Value.Item1, d => d.Value.Item2);
        }
    }
}