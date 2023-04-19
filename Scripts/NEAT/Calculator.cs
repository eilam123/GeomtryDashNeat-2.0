using System.Collections.Generic;

namespace NEAT
{
    public class Calculator 
    {
        private List<Node> inputNodes = new List<Node>();
        private List<Node> outputNodes = new List<Node>();
        private List<Node> hiddenNodes = new List<Node>();

        public Calculator(Genome g)
        {
            RandomHashSet<NodeGene> nodes = g.GetNodes();
            RandomHashSet<ConnectionGene> cons = g.GetConnections();
            Dictionary<int, Node> nodeHashMap = new Dictionary<int, Node>();

            foreach (NodeGene n in nodes.GetDataSet())
            {
                Node node = new Node(n.GetX());
                nodeHashMap.Add(n.GetInnovationNumber(), node);
                
                if(n.GetX() <= 0.1)
                    inputNodes.Add(node);
                else if(n.GetX() >= 0.9)
                    outputNodes.Add(node);
                else
                    hiddenNodes.Add(node);
            }
            hiddenNodes.Sort((Node a, Node b) => a.CompareTo(b));

            foreach (ConnectionGene c in cons.GetDataSet())
            {
                NodeGene from = c.GetFrom();
                NodeGene to = c.GetTo();
                Node fromNode = nodeHashMap[from.GetInnovationNumber()];
                Node toNode = nodeHashMap[to.GetInnovationNumber()];
                Connection con = new Connection(fromNode, toNode);
                con.SetWeight(c.GetWeight());
                con.SetEnabled(c.IsEnabled());
                
                toNode.GetConnections().Add(con);
            }
        }

        public double[] Calculate(params double[] input)
        {
            if (input.Length != inputNodes.Count)
                throw new System.Exception("Input size does not match the input size of the genome");
            for(int i = 0; i < inputNodes.Count; i++)
            {
                inputNodes[i].SetOutput(input[i]);
            }
            foreach(Node n in hiddenNodes)
            {
                n.Calculate();
            }
            double[] output = new double[outputNodes.Count];
            for(int i = 0; i < outputNodes.Count; i++)
            {
                outputNodes[i].Calculate();
                output[i] = outputNodes[i].GetOutput();
            }
            return output;
        }
        
    }
}
