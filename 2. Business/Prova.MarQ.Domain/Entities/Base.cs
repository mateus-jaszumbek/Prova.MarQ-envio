namespace Prova.MarQ.Domain.Entities
{
    public abstract class Base
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
