namespace NEAT
{
    public class Connection
    {
        private Node from;
        private Node to;
        private double weight;
        private bool enabled = true;

        public Connection(Node from, Node to)
        {
            this.from = from;
            this.to = to;
        }

        public void SetFrom(Node from)
        {
            this.from = from;
        }

        public void SetTo(Node to)
        {
            this.to = to;
        }

        public Node GetFrom()
        {
            return from;
        }

        public Node GetTo()
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
    }
}