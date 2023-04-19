using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace NEAT
{
    public class RandomHashSet<T>
    {
        HashSet<T> set;
        List<T> data;

        public RandomHashSet()
        {
            set = new HashSet<T>();
            data = new List<T>();
        }

        public bool Contains(T item)
        {
            return set.Contains(item);
        }

        public T RandomElement()
        {
            if (data.Count > 0)
                return data[Random.Range(0, data.Count)];
            else
                return default(T);
        }

        public int Size()
        {
            return data.Count;
        }

        public void Add(T item)
        {
            if (!set.Contains(item))
            {
                set.Add(item);
                data.Add(item);
            }
        }

        public void Clear()
        {
            set.Clear();
            data.Clear();
        }

        public T Get(int index)
        {
            if (index < 0 || index >= data.Count)
                return default(T);
            return data[index];
        }

        public T Get(T template)
        {
            return data[data.IndexOf(template)];
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= data.Count)
                return;
            set.Remove(data[index]);
            data.RemoveAt(index);
        }

        public void Remove(T item)
        {
            set.Remove(item);
            data.Remove(item);
        }

        public List<T> GetDataSet()
        {
            return data;
        }

        public void AddSorted(Gene obj)
        {
            for (int i = 0; i < this.Size(); i++)
            {
                int innovation = GetGeneAtIndex(i).GetInnovationNumber();
                if (obj.GetInnovationNumber() < innovation)
                {
                    AddGene(obj, i);
                    return;
                }
            }
            AddGene(obj, this.Size());
        }
        private Gene GetGeneAtIndex(int index)
        {
            // Cast the element at the specified index to Gene
            Gene gene = (Gene)Convert.ChangeType(data[index], typeof(Gene));
            return gene;
        }
        
        private void AddGene(Gene gene, int index)
        {
            // Cast Gene to T and add to data list and set HashSet
            T item = (T)(object)gene;
            data.Insert(index, item);
            set.Add(item);
        }
    }
}