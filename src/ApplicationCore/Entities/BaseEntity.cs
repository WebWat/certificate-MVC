using System;

namespace ApplicationCore.Entities;

public abstract class BaseEntity
{
    public virtual string Id { get; protected set; } = Guid.NewGuid().ToString();

    //[JsonProperty(PropertyName = "partition")]
    //public string Partition { get; set; }
}
