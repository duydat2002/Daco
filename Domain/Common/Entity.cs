namespace Daco.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        protected Entity() { }

        protected Entity(Guid id)
        {
            Id = id;
        }

        internal void SetId(Guid id)
        {
            if (Id != Guid.Empty && Id != id)
                throw new InvalidOperationException("Cannot change existing Id");
            Id = id;
        }
    }
}
