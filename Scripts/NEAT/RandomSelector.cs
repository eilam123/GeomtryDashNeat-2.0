using System.Collections.Generic;

namespace NEAT
{
    public class RandomSelector<T>
    {
        private List<T> objects;
        private List<double> scores;
    
        private double totalScore = 0;
    
        public void Add(T obj, double score)
        {
            objects.Add(obj);
            scores.Add(score);
            totalScore += score;
        }

        public T Random()
        {
            double v = totalScore * UnityEngine.Random.value;
            double c = 0;
            for (int i = 0; i < objects.Count; i++)
            {
                c += scores[i];
                if (c > v)
                    return objects[i];
            }
            return default(T);
        }

        public void Reset()
        {
            objects.Clear();
            scores.Clear();
            totalScore = 0;
        }
    }
}
