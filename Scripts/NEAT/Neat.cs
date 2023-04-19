using System.Collections.Generic;

namespace NEAT
{
    public class Neat
    {
        public const int MAX_NODES = 1046576;
        private double SURVIVORS = 0.8;
        private double C1 = 1, C2 = 1, C3 = 1;
        private double CP = 4;
        private double WIEGHT_SHIFT_STRENGTH = 0.3;
        private double WIEGHT_RANDOM_STRENGTH = 1;
        private double PROBABILITY_MUTATE_LINK = 0.01;
        private double PROBABILITY_MUTATE_NODE = 0.03;
        private double PROBABILITY_MUTATE_WEIGHT_SHIFT = 0.02;
        private double PROBABILITY_MUTATE_WEIGHT_RANDOM = 0.02;
        private double PROBABILITY_MUTATE_TOGGLE_LINK = 0;

        private Dictionary<ConnectionGene, ConnectionGene>
            allConnections = new Dictionary<ConnectionGene, ConnectionGene>();

        private RandomHashSet<NodeGene> allNodes = new RandomHashSet<NodeGene>();
        private RandomHashSet<Client> clients = new RandomHashSet<Client>();
        private RandomHashSet<Species> species = new RandomHashSet<Species>();
        private int maxClients;
        private int inputSize;
        private int outputSize;

        public Neat(int inputSize, int outputSize, int clients)
        {
            Reset(inputSize, outputSize, clients);
        }
        public Client GetBestClient()
        {
            int best = 0;
            for (int i = 0; i < clients.Size(); i++)
            {
                if (clients.Get(i).GetScore() > clients.Get(best).GetScore())
                {
                    best = i;
                }
            }
            return clients.Get(best);
        }
        public Genome EmptyGenom()
        {
            Genome g = new Genome(this);
            for (int i = 0; i < inputSize + outputSize; i++)
            {
                g.GetNodes().Add(GetNode(i + 1));
            }

            return g;
        }

        public void Reset(int inputSize, int outputSize, int clients)
        {
            this.inputSize = inputSize;
            this.outputSize = outputSize;
            maxClients = clients;
            allConnections.Clear();
            allNodes.Clear();
            this.clients.Clear();
            for (int i = 0; i < inputSize; i++)
            {
                NodeGene n = GetNode();
                n.SetX(0.1);
                n.SetY((i + 1) / (double)(inputSize + 1));
            }

            for (int i = 0; i < outputSize; i++)
            {
                NodeGene n = GetNode();
                n.SetX(0.9);
                n.SetY((i + 1) / (double)(outputSize + 1));
            }

            for (int i = 0; i < maxClients; i++)
            {
                Client c = new Client();
                c.SetGenome(EmptyGenom());
                c.GenerateCalculator();
                this.clients.Add(c);
            }
        }

        public Client GetClient(int index)
        {
            return clients.Get(index);
        }

        public NodeGene GetNode()
        {
            NodeGene n = new NodeGene(allNodes.Size() + 1);
            allNodes.Add(n);
            return n;
        }

        public NodeGene GetNode(int id)
        {
            if (id > 0 && id <= allNodes.Size())
                return allNodes.Get(id - 1);
            return GetNode();
        }

        public ConnectionGene GetConnection(ConnectionGene con)
        {
            ConnectionGene cg = new ConnectionGene(con.GetFrom(), con.GetTo());
            cg.SetWeight(con.GetWeight());
            cg.SetInnovationNumber(con.GetInnovationNumber());
            cg.SetEnabled(con.IsEnabled());
            return cg;
        }

        public ConnectionGene GetConnection(NodeGene from, NodeGene to)
        {
            ConnectionGene c = new ConnectionGene(from, to);
            if (allConnections.ContainsKey(c))
            {
                c.SetInnovationNumber(allConnections[c].GetInnovationNumber());
            }
            else
            {
                c.SetInnovationNumber(allConnections.Count + 1);
                allConnections.Add(c, c);
            }

            return c;
        }

        public void SetReplaceIndex(NodeGene node1, NodeGene node2, int index)
        {
            allConnections[new ConnectionGene(node1, node2)].SetReplaceIndex(index);
        }
        public int GetReplaceIndex(NodeGene node1, NodeGene node2)
        {
            ConnectionGene con = new ConnectionGene(node1, node2);
            ConnectionGene data = allConnections[con];
            if (data == null)
                return 0;
            return data.GetReplaceIndex();
        }

        public double GetC1()
        {
            return C1;
        }

        public double GetC2()
        {
            return C2;
        }

        public double GetC3()
        {
            return C3;
        }

        public double GetWIEGHT_SHIFT_STRENGTH()
        {
            return WIEGHT_SHIFT_STRENGTH;
        }

        public double GetWIEGHT_RANDOM_STRENGTH()
        {
            return WIEGHT_RANDOM_STRENGTH;
        }

        public double GetPROBABILITY_MUTATE_LINK()
        {
            return PROBABILITY_MUTATE_LINK;
        }

        public double GetPROBABILITY_MUTATE_NODE()
        {
            return PROBABILITY_MUTATE_NODE;
        }

        public double GetPROBABILITY_MUTATE_WEIGHT_SHIFT()
        {
            return PROBABILITY_MUTATE_WEIGHT_SHIFT;
        }

        public double GetPROBABILITY_MUTATE_WEIGHT_RANDOM()
        {
            return PROBABILITY_MUTATE_WEIGHT_RANDOM;
        }

        public double GetPROBABILITY_MUTATE_TOGGLE_LINK()
        {
            return PROBABILITY_MUTATE_TOGGLE_LINK;
        }

        public double GetCP()
        {
            return CP;
        }

        public void SetCP(double CP)
        {
            this.CP = CP;
        }

        public void Evolve()
        {
            GenerateSpecies();
            Kill();
            RemoveExtinctSpecies();
            Reproduce();
            Mutate();
            foreach (Client c in clients.GetDataSet())
            {
                c.GenerateCalculator();
            }
        }

        private void GenerateSpecies()
        {
            foreach (Species s in species.GetDataSet())
            {
                s.Reset();
            }

            foreach (Client c in clients.GetDataSet())
            {
                if (c.GetSpecies() != null)
                    continue;
                bool found = false;
                foreach (Species s in species.GetDataSet())
                {
                    if (s.Put(c))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    species.Add(new Species(c));
                }
            }

            foreach (Species s in species.GetDataSet())
            {
                s.EvaluateScore();
            }
        }

        private void Kill()
        {
            foreach (Species s in species.GetDataSet())
            {
                s.Kill(1 - SURVIVORS);
            }
        }

        private void RemoveExtinctSpecies()
        {
            for (int i = species.Size() - 1; i >= 0; i--)
            {
                if (species.Get(i).Size() <= 1)
                {
                    species.Get(i).GoExtinct();
                    species.Remove(i);
                }
            }
        }

        private void Reproduce()
        {
            RandomSelector<Species> selector = new RandomSelector<Species>();
            foreach (Species s in species.GetDataSet())
            {
                selector.Add(s, s.GetScore());
            }

            foreach (Client c in clients.GetDataSet())
            {
                if (c.GetSpecies() == null)
                {
                    Species s = selector.Random();
                    c.SetGenome(s.Breed());
                    s.ForcePut(c);
                }
            }
        }

        public void Mutate()
        {
            foreach (Client c in clients.GetDataSet())
            {
                c.Mutate();
            }
        }
    }
}