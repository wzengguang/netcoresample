namespace Sample.Domain
{
    public class EntityCreated : EntityBase, IEntityCreated
    {
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        public long? CreatedUserId { get; set; }
    }

    public interface IEntityCreated
    {
        DateTime CreatedTime { get; set; }

        DateTime UpdatedTime { get; set; }

        long? CreatedUserId { get; set; }
    }
}
