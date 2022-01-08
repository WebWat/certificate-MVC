using ApplicationCore.Entities;
using System.Collections.Generic;

namespace IntegrationTests.Builders;

public class LinkBuilder
{
    public static List<Link> GetDefaultValues(int certificateId = 0)
    {
        return new List<Link>
            {
                new Link ("https://example.com", certificateId),
                new Link ("https://example.com/", certificateId)
            };
    }
}
