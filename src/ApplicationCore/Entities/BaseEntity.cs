using Newtonsoft.Json;

namespace ApplicationCore.Entities;

public abstract class BaseEntity
{
    public virtual int Id { get; protected set; }

    //[JsonProperty(PropertyName = "partition")]
    //public string Partition { get; set; }
}
