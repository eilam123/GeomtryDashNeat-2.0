using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NEAT
{
    public class Genome
    {
        private RandomHashSet<ConnectionGene> connections = new RandomHashSet<ConnectionGene>();
        private RandomHashSet<NodeGene> nodes = new RandomHashSet<NodeGene>();
        private Neat neat;

        public Genome(Neat neat)
        {
            this.neat = neat;
        }

        public RandomHashSet<ConnectionGene> GetConnections()
        {
            return connections;
        }

        public RandomHashSet<NodeGene> GetNodes()
        {
            return nodes;
        }

        public Neat GetNeat()
        {
            return neat;
        }

        // calculate the distance between genome1 and genome2  
        // genome1 must have a higher innovation number than genome2
        public double Distance(Genome g2)
        {
            Genome g1 = this;
            int highest_innovation_gene1 = 0;
            if(g1.GetConnections().Size() != 0){
                highest_innovation_gene1 = g1.GetConnections().Get(g1.GetConnections().Size()-1).GetInnovationNumber();
            }

            int highest_innovation_gene2 = 0;
            if(g2.GetConnections().Size() != 0){
                highest_innovation_gene2 = g2.GetConnections().Get(g2.GetConnections().Size()-1).GetInnovationNumber();
            }
            if(highest_innovation_gene1 < highest_innovation_gene2){
                Genome g = g1;
                g1 = g2;
                g2 = g;
            }
            int indexG1 = 0;
            int indexG2 = 0;

            int disjoint = 0;
            int excess = 0;
            double weightDiff = 0;
            int similar = 0;

            while (indexG1 < g1.GetConnections().Size() && indexG2 < g2.GetConnections().Size())
            {
               
                ConnectionGene c1 = g1.GetConnections().Get(indexG1);
                ConnectionGene c2 = g2.GetConnections().Get(indexG2);
                int in1 = c1.GetInnovationNumber();
                int in2 = c2.GetInnovationNumber();
                if (in1 == in2)
                {
                    indexG1++;
                    indexG2++;
                    similar++;
                    weightDiff += Mathf.Abs((float)(c1.GetWeight() - c2.GetWeight()));
                }
                else if (in1 > in2)
                {
                    indexG2++;
                    disjoint++;
                }
                else
                {
                    indexG1++;
                    disjoint++;
                }
            }

            weightDiff /= Math.Max(1, similar);
            excess = g1.GetConnections().Size() - indexG1;
            double N = Math.Max(g1.GetConnections().Size(), g2.GetConnections().Size());
            if (N < 20)
            {
                N = 1;
            }

            return neat.GetC1() * disjoint / N + neat.GetC2() * excess / N + neat.GetC3() * weightDiff;
        }

        public static Genome CrossOver(Genome g1, Genome g2)
        {
            Neat neat = g1.GetNeat();
            Genome genome = neat.EmptyGenom();
            int indexG1 = 0;
            int indexG2 = 0;
            while (indexG1 < g1.GetConnections().Size() && indexG2 < g2.GetConnections().Size())
            {
                ConnectionGene gene1 = g1.GetConnections().Get(indexG1);
                ConnectionGene gene2 = g2.GetConnections().Get(indexG2);
                int in1 = gene1.GetInnovationNumber();
                int in2 = gene2.GetInnovationNumber();
                if (in1 == in2)
                {
                    if (Random.value > 0.5)
                    {
                        genome.GetConnections().Add(neat.GetConnection(gene1));
                    }
                    else
                    {
                        genome.GetConnections().Add(neat.GetConnection(gene2));
                    }

                    indexG1++;
                    indexG2++;
                }
                else if (in1 > in2)
                {
                    indexG2++;
                    //genome.GetConnections().Add(neat.GetConnection(gene2));
                }
                else
                {
                    genome.GetConnections().Add(neat.GetConnection(gene1));
                    indexG1++;
                }
            }

            while (indexG1 < g1.GetConnections().Size())
            {
                ConnectionGene gene1 = g1.GetConnections().Get(indexG1);
                genome.GetConnections().Add(neat.GetConnection(gene1));
                indexG1++;
            }

            foreach (ConnectionGene c in genome.GetConnections().GetDataSet())
            {
                genome.GetNodes().Add(c.GetFrom());
                genome.GetNodes().Add(c.GetTo());
            }

            return genome;
        }

        public void MutateLink()
        {
            for (int i = 0; i < 100; i++)
            {
                NodeGene a = nodes.RandomElement();
                NodeGene b = nodes.RandomElement();

                if (a.GetX() == b.GetX())
                {
                    continue;
                }

                ConnectionGene con;
                if (a.GetX() < b.GetX())
                {
                    con = new ConnectionGene(a, b);
                }
                else
                {
                    con = new ConnectionGene(b, a);
                }

                if (connections.Contains(con))
                {
                    continue;
                }
                con = neat.GetConnection(con.GetFrom(), con.GetTo());
                con.SetWeight((Random.value * 2 - 1) * neat.GetWIEGHT_RANDOM_STRENGTH());
                connections.AddSorted(con);
                return;
            }
        }

        public void MutateWeightShift()
        {
            ConnectionGene con = connections.RandomElement();
            if (con != null)
            {
                con.SetWeight(con.GetWeight() + (Random.value * 2 - 1) * neat.GetWIEGHT_SHIFT_STRENGTH());
            }
        }

        public void MutateLinkToggle()
        {
            ConnectionGene con = connections.RandomElement();
            if (con != null)
            {
                con.SetEnabled(!con.IsEnabled());
            }
        }

        public void MutateWeightRandom()
        {
            ConnectionGene con = connections.RandomElement();
            if (con != null)
            {
                con.SetWeight((Random.value * 2 - 1) * neat.GetWIEGHT_RANDOM_STRENGTH());
            }
        }

        public void MutateNode()
        {
            ConnectionGene con = connections.RandomElement();
            if(con == null)
            {
                return;
            }
            NodeGene from = con.GetFrom();
            NodeGene to = con.GetTo();
            int replaceIndex = neat.GetReplaceIndex(from, to);
            //replaceIndex = 0;
            NodeGene middle;
            if (replaceIndex == 0)
            {
                middle = neat.GetNode();
                middle.SetX((from.GetX() + to.GetX()) / 2);
                middle.SetY((from.GetY() + to.GetY()) / 2 + Random.value * 0.1f - 0.05f);
                neat.SetReplaceIndex(from, to, middle.GetInnovationNumber());
            }
            else
            {
                middle = neat.GetNode(replaceIndex);
            }
            
            
            ConnectionGene con1 = neat.GetConnection(from, middle);
            ConnectionGene con2 = neat.GetConnection(middle, to);
            
            con1.SetWeight(1);
            con2.SetWeight(con.GetWeight());
            con2.SetEnabled(con.IsEnabled());
            
            connections.Remove(con);
            connections.Add(con1);
            connections.Add(con2);
            nodes.Add(middle);
        }
        
        public void Mutate()
        {
            if (Random.value < neat.GetPROBABILITY_MUTATE_LINK())
            {
                MutateLink();
            }
            if (Random.value < neat.GetPROBABILITY_MUTATE_WEIGHT_SHIFT())
            {
                MutateWeightShift();
            }
            if (Random.value < neat.GetPROBABILITY_MUTATE_TOGGLE_LINK())
            {
                MutateLinkToggle();
            }
            if (Random.value < neat.GetWIEGHT_RANDOM_STRENGTH())
            {
                MutateWeightRandom();
            }
            if (Random.value < neat.GetPROBABILITY_MUTATE_NODE())
            {
                MutateNode();
            }
        }
    }
}