using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARSIE.DataIO
{
    public class CombinationGenerator<T>
    {
        public IEnumerable<List<T>> ProduceWithRecursion(List<T> allValues)
        {
            for (var i = 0; i < (1 << allValues.Count); i++)
            {
                yield return ConstructSetFromBits(i).Select(n => allValues[n]).ToList();
            }
        }

        private IEnumerable<int> ConstructSetFromBits(int i)
        {
            var n = 0;
            for (; i != 0; i /= 2)
            {
                if ((i & 1) != 0) yield return n;
                n++;
            }
        }

        public List<List<T>> ProduceWithoutRecursion(List<T> allValues)
        {
            var collection = new List<List<T>>();
            for (int counter = 0; counter < (1 << allValues.Count); ++counter)
            {
                List<T> combination = new List<T>();
                for (int i = 0; i < allValues.Count; ++i)
                {
                    if ((counter & (1 << i)) == 0)
                        combination.Add(allValues[i]);
                }

                // do something with combination
                collection.Add(combination);
            }
            return collection;
        }
    }
}
