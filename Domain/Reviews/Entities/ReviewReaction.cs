namespace Daco.Domain.Reviews.Entities
{
    public class ReviewReaction : Entity
    {
        public Guid     ReviewId  { get; private set; }
        public Guid     UserId    { get; private set; }
        public bool     IsHelpful { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected ReviewReaction() { }
    }
}
