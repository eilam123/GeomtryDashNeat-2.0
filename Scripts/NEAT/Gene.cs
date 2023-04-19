namespace NEAT
{
    public class Gene
    {
        protected int InnovationNumber;

        public Gene(int innovationNumber)
        {
            InnovationNumber = innovationNumber;
        }

        public Gene()
        {
        
        }

        public int GetInnovationNumber()
        {
            return InnovationNumber;
        }

        public void SetInnovationNumber(int innovationNumber)
        {
            InnovationNumber = innovationNumber;
        }
    }
}