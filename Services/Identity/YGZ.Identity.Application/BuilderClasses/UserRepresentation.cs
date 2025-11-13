using Newtonsoft.Json;

namespace YGZ.Identity.Application.BuilderClasses;

public class UserRepresentation
{
    [JsonProperty("username")]
    public string Username { get; set; } = default!;

    [JsonProperty("firstName")]
    public string? FirstName { get; set; }

    [JsonProperty("lastName")]
    public string? LastName { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; } = default!;

    [JsonProperty("emailVerified")]
    public bool EmailVerified { get; set; }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; } = true;

    [JsonProperty("credentials")]
    public List<CredentialRepresentation>? Credentials { get; set; }

    [JsonProperty("attributes")]
    public Dictionary<string, List<string>>? Attributes { get; set; }

    public static UserRepresentationBuilder CreateBuilder()
    {
        return new UserRepresentationBuilder();
    }
}

public class CredentialRepresentation
{
    [JsonProperty("type")]
    public string Type { get; set; } = "password";

    [JsonProperty("value")]
    public string Value { get; set; } = default!;

    [JsonProperty("temporary")]
    public bool Temporary { get; set; } = false;
}

public class UserRepresentationBuilder
{
    private readonly UserRepresentation _userRepresentation;

    public UserRepresentationBuilder()
    {
        _userRepresentation = new UserRepresentation
        {
            Enabled = true,
            EmailVerified = false,
            Credentials = new List<CredentialRepresentation>(),
            Attributes = new Dictionary<string, List<string>>()
        };
    }

    public UserRepresentationBuilder WithUsername(string username)
    {
        _userRepresentation.Username = username;
        return this;
    }

    public UserRepresentationBuilder WithFirstName(string firstName)
    {
        _userRepresentation.FirstName = firstName;
        return this;
    }

    public UserRepresentationBuilder WithLastName(string lastName)
    {
        _userRepresentation.LastName = lastName;
        return this;
    }

    public UserRepresentationBuilder WithEmail(string email)
    {
        _userRepresentation.Email = email;
        return this;
    }

    public UserRepresentationBuilder WithEmailVerified(bool emailVerified)
    {
        _userRepresentation.EmailVerified = emailVerified;
        return this;
    }

    public UserRepresentationBuilder WithEnabled(bool enabled)
    {
        _userRepresentation.Enabled = enabled;
        return this;
    }

    public UserRepresentationBuilder WithPassword(string password, bool temporary = false)
    {
        _userRepresentation.Credentials ??= new List<CredentialRepresentation>();
        _userRepresentation.Credentials.Add(new CredentialRepresentation
        {
            Type = "password",
            Value = password,
            Temporary = temporary
        });
        return this;
    }

    public UserRepresentationBuilder WithCredential(string type, string value, bool temporary = false)
    {
        _userRepresentation.Credentials ??= new List<CredentialRepresentation>();
        _userRepresentation.Credentials.Add(new CredentialRepresentation
        {
            Type = type,
            Value = value,
            Temporary = temporary
        });
        return this;
    }

    private UserRepresentationBuilder WithAttribute(string key, string value)
    {
        _userRepresentation.Attributes ??= new Dictionary<string, List<string>>();

        if (_userRepresentation.Attributes.ContainsKey(key))
        {
            _userRepresentation.Attributes[key].Add(value);
        }
        else
        {
            _userRepresentation.Attributes[key] = new List<string> { value };
        }

        return this;
    }

    public UserRepresentationBuilder WithAttributes(Dictionary<string, List<string>> attributes)
    {
        _userRepresentation.Attributes = attributes;
        return this;
    }

    public UserRepresentationBuilder WithTenantId(string tenantId)
    {
        return WithAttribute("tenant_id", tenantId);
    }

    public UserRepresentationBuilder WithSubDomain(string subDomain)
    {
        return WithAttribute("sub_domain", subDomain);
    }

    public UserRepresentationBuilder WithTenantType(string tenantType)
    {
        return WithAttribute("tenant_type", tenantType);
    }

    public UserRepresentationBuilder WithBranchId(string branchId)
    {
        return WithAttribute("branch_id", branchId);
    }

    public UserRepresentationBuilder WithFullName(string fullName)
    {
        return WithAttribute("full_name", fullName);
    }

    /// <summary>
    /// Adds all tenant-related attributes at once
    /// </summary>
    public UserRepresentationBuilder WithTenantAttributes(string? tenantId, string? subDomain, string? tenantType, string? branchId, string? fullName)
    {
        return WithTenantId(tenantId ?? "")
               .WithSubDomain(subDomain ?? "")
               .WithTenantType(tenantType ?? "")
               .WithBranchId(branchId ?? "")
               .WithFullName(fullName ?? "");
    }

    public UserRepresentation Build()
    {
        if (string.IsNullOrEmpty(_userRepresentation.Username))
            throw new InvalidOperationException("Username is required");

        if (string.IsNullOrEmpty(_userRepresentation.Email))
            throw new InvalidOperationException("Email is required");

        return _userRepresentation;
    }
}
