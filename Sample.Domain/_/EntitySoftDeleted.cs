namespace Sample.Domain
{
    public class EntitySoftDeleted : EntityBase, IEntitySoftDeleted
    {
        public bool IsDeleted { get; set; }
        public DateTime DeletedTime { get; set; } = DateTime.Now;
    }

    public interface IEntitySoftDeleted
    {
        bool IsDeleted { get; set; }

        DateTime DeletedTime { get; set; }
    }
}
