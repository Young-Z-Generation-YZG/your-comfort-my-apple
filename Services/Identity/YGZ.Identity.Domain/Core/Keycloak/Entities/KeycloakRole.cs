using Newtonsoft.Json;

namespace YGZ.Identity.Domain.Core.Keycloak.Entities;

public class KeycloakRole
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("composite")]
    public bool Composite { get; set; }

    [JsonProperty("clientRole")]
    public bool ClientRole { get; set; }

    [JsonProperty("containerId")]
    public string? ContainerId { get; set; }

    [JsonProperty("attributes")]
    public Dictionary<string, List<string>>? Attributes { get; set; }

    public static KeycloakRole Create(string id, string name, string? description = null, bool composite = false, bool clientRole = true)
    {
        return new KeycloakRole
        {
            Id = id,
            Name = name,
            Description = description ?? string.Empty,
            Composite = composite,
            ClientRole = clientRole,
            Attributes = new Dictionary<string, List<string>>()
        };
    }
}
