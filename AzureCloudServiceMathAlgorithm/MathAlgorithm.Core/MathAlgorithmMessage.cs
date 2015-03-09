namespace MathAlgorithm.Core
{
    public class MathAlgorithmMessage
    {
        public enum QueueType
        {
            Calculate,
            Save
        }

        public int MessageId { get; set;}
        public QueueType Type { get; set; }
    }
}
