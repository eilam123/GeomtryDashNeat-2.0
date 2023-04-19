using UnityEngine;

namespace NEAT
{
    public class Client
    {
        private Genome genome;
        private Species species;
        private double score;
        private Calculator calculator;

        public void GenerateCalculator()
        {
            calculator = new Calculator(genome);
        }

        public double[] Calculate(params double[] inputs)
        {
            if (calculator == null)
                GenerateCalculator();
            return calculator.Calculate(inputs);
        }

        public double Distance(Client client)
        {
            return genome.Distance(client.GetGenome());
        }

        public void Mutate()
        {
            genome.Mutate();
        }

        public Genome GetGenome()
        {
            return genome;
        }

        public void SetGenome(Genome genome)
        {
            this.genome = genome;
        }

        public Species GetSpecies()
        {
            return species;
        }

        public void SetSpecies(Species species)
        {
            this.species = species;
        }

        public double GetScore()
        {
            return score;
        }

        public void SetScore(double score)
        {
            this.score = score;
        }

        public Calculator GetCalculator()
        {
            return calculator;
        }
    }
}