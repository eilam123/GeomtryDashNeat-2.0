namespace NEAT
{
    public class ConnectionGene : Gene
    {
        private NodeGene from;
        private NodeGene to;
        private double weight;
        private bool enabled = true;
        private int ReplaceIndex;

        public ConnectionGene(NodeGene from, NodeGene to)
        {
            this.from = from;
            this.to = to;
        }

        public void SetReplaceIndex(int index)
        {
            ReplaceIndex = index;
        }

        public int GetReplaceIndex()
        {
            return ReplaceIndex;
        }

        public void SetFrom(NodeGene from)
        {
            this.from = from;
        }

        public void SetTo(NodeGene to)
        {
            this.to = to;
        }

        public NodeGene GetFrom()
        {
            return from;
        }

        public NodeGene GetTo()
        {
            return to;
        }

        public void SetWeight(double weight)
        {
            this.weight = weight;
        }

        public double GetWeight()
        {
            return weight;
        }

        public void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
        }

        public bool IsEnabled()
        {
            return enabled;
        }

        public bool Equals(object other)
        {
            if (other == null || !(other is ConnectionGene))
            {
                return false;
            }

            ConnectionGene otherObject = (ConnectionGene)other;
            return GetFrom().Equals(otherObject.GetFrom()) && GetTo().Equals(otherObject.GetTo());
        }

        public int HashCode()
        {
            return GetFrom().GetInnovationNumber() * Neat.MAX_NODES + GetTo().GetInnovationNumber();
        }
    }
}