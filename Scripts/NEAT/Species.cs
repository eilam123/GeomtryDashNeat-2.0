namespace NEAT
{
    public class Species
    {
        private RandomHashSet<Client> clients = new RandomHashSet<Client>();
        private Client representative;
        private double score;

        public Species(Client representative)
        {
            this.representative = representative;
            this.representative.SetSpecies(this);
            clients.Add(representative);
        }

        public bool Put(Client client)
        {
            if (client.Distance(representative) < representative.GetGenome().GetNeat().GetCP())
            {
                client.SetSpecies(this);
                clients.Add(client);
                return true;
            }

            return false;
        }

        public void ForcePut(Client client)
        {
            client.SetSpecies(this);
            clients.Add(client);
        }

        public void GoExtinct()
        {
            foreach (Client client in clients.GetDataSet())
            {
                client.SetSpecies(null);
            }
        }

        public void EvaluateScore()
        {
            double v = 0;
            foreach (Client client in clients.GetDataSet())
            {
                v += client.GetScore();
            }

            score = v / clients.Size();
        }

        public void Reset()
        {
            representative = clients.RandomElement();
            foreach (Client client in clients.GetDataSet())
            {
                client.SetSpecies(null);
            }

            clients.Clear();
            clients.Add(representative);
            representative.SetSpecies(this);
            score = 0;
        }

        public void Kill(double percentage)
        {
            clients.GetDataSet().Sort((client1, client2) => client1.GetScore().CompareTo(client2.GetScore()));
            double size = (clients.Size() * percentage);
            for (int i = 0; i < size; i++)
            {
                clients.Get(0).SetSpecies(null);
                clients.Remove(0);
            }
        }
        
        public Genome Breed()
        {
            Client c1 = clients.RandomElement();
            Client c2 = clients.RandomElement();
            if(c1.GetScore() > c2.GetScore())
                return Genome.CrossOver(c1.GetGenome(), c2.GetGenome());
            return Genome.CrossOver(c2.GetGenome(), c1.GetGenome());
            
        }
        
        public int Size()
        {
            return clients.Size();
        }
        
        public double GetScore()
        {
            return score;
        }
        
        public Client GetRepresentative()
        {
            return representative;
        }
        
        public RandomHashSet<Client> GetClients()
        {
            return clients;
        }
    }
}