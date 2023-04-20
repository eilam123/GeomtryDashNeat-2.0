namespace NEAT
{
    public class NodeGene : Gene
    {
        private double x, y;

        public NodeGene(int innovationNumber) : base(innovationNumber)
        {
        }

        public double GetX()
        {
            return x;
        }

        public double GetY()
        {
            return y;
        }

        public void SetX(double x)
        {
            this.x = x;
        }

        public void SetY(double y)
        {
            this.y = y;
        }

        public override bool Equals(object other)
        {
            if (other == null || !(other is NodeGene))
            {
                return false;
            }

            NodeGene otherObject = (NodeGene)other;
            return GetInnovationNumber() == otherObject.GetInnovationNumber();
        }
        public int HashCode()
        {
            return InnovationNumber;
        }
    }
}