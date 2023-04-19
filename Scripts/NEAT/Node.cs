using System;
using System.Collections.Generic;

namespace NEAT
{
    public class Node : IComparable<Node>
    {
        private double x;
        private double output;
        private List<Connection> connections = new List<Connection>();

        public void Calculate()
        {
            double s = 0;
            foreach (Connection c in connections)
            {
                if (c.IsEnabled())
                    s += c.GetFrom().GetOutput() * c.GetWeight();
            }
            output = ActivationFunction(s);
        }
        public double ActivationFunction(double x)
        {
            return 1.0 / (1.0 + System.Math.Exp(-x));
        }
    
        public int CompareTo(Node other)
        {
            if (x < other.x)
                return 1;
            else if (x > other.x)
                return -1;
            else
                return 0;
        }
        public Node(double x)
        {
            this.x = x;
        }

        public void SetX(double x)
        {
            this.x = x;
        }

        public double GetX()
        {
            return x;
        }

        public void SetOutput(double output)
        {
            this.output = output;
        }

        public double GetOutput()
        {
            return output;
        }

        public List<Connection> GetConnections()
        {
            return connections;
        }

        public void SetConnections(List<Connection> connections)
        {
            this.connections = connections;
        }
    }
}