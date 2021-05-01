using System.Text.Json.Serialization;

namespace MLambda.Image.Service.model
{
  public class Credential
  {
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; }

    [JsonPropertyName("clientSecret")]
    public string ClientSecret { get; set; }
  }
}
