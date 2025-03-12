namespace AdvancedDevelopment.Models
{
    public class Review
    {
        public int Rating { get; set; }         // Rating given by the reviewer (1-5)
        public string Comment { get; set; }     // Review Comment
        public string ReviewerName { get; set; }// Name of the reviewer
    }
}