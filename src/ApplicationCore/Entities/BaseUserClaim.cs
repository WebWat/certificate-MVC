namespace ApplicationCore.Entities
{
    /// <summary>
    /// Uses the UserId property to verify the user
    /// </summary>
    public abstract class BaseUserClaim : BaseEntity
    {
        public virtual string UserId { get; set; }
    }
}
