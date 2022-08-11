namespace Sample.Domain
{
    public class EntitySoftDeletedAndCreated : EntityBase, IEntitySoftDeletedAndCreated, IEntitySoftDeleted, IEntityCreated
    {
        public bool IsDeleted { get; set; }

        public DateTime DeletedTime { get; set; }

        public DateTime CreatedTime { get; set; }

        public long? CreatedUserId { get; set; }
        public DateTime UpdatedTime { get; set; }
    }

    public interface IEntitySoftDeletedAndCreated : IEntitySoftDeleted, IEntityCreated, IEntityBase
    {

    }
}
