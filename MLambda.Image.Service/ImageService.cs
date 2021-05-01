using System;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Text.Encodings.Web;
using MLambda.Image.Abstract;
using MLambda.Image.Service.model;
using RestSharp;
using RestSharp.Serialization.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MLambda.Image.Service
{
  public class ImageService : IImageService
  {
    private readonly IOptions options;
    private readonly RestClient client;

    public ImageService(IOptions options)
    {
      this.options = options;
      this.client = new RestClient(options.Server);
    }

    private IObservable<AuthToken> Token()
    {
      var data = new Credential
      {
        ClientId = options.ClientId,
        ClientSecret = options.ClientSecret
      };

      var request = new RestRequest("token", Method.POST);
      request.AddHeader("content-type", "application/json");
      request.AddParameter("application/json", JsonSerializer.Serialize(data), ParameterType.RequestBody);

      var response = this.client.Execute<AuthToken>(request).Data;
      return Observable.Return(response);
    }

    public IObservable<bool> Delete(string img)
    {
      return this.Token().Select(c =>
      {
        var request = new RestRequest($"/files/delete?filename=/{img}", Method.POST);
        request.AddHeader("authorization", $"Bearer {c.Token}");
        request.AddHeader("content-type", "application/json");
        var response = client.Execute(request);
        return response.IsSuccessful;
      });
    }

    public IObservable<bool> CreateDirectory(string name)
    {
      return this.Token().Select(c =>
      {
        var request = new RestRequest($"/files/mkdir?dirname=/{name}", Method.POST);
        request.AddHeader("authorization", $"Bearer {c.Token}");
        request.AddHeader("content-type", "application/json");
        var response = client.Execute(request);
        return response.IsSuccessful;
      });
    }

    public IObservable<string> Upload(string directory, string fileName, string type, byte[] file)
    {
      return this.Token().Select(c =>
      {
        var name = WebUtility.UrlEncode($"/{directory}/{fileName}");
        var request = new RestRequest($"/files/upload?filename={name}", Method.POST);
        request.AddHeader("authorization", $"Bearer {c.Token}");
        request.AddHeader("content-type", type);
        request.AddParameter("data", file, ParameterType.RequestBody);
        var response = client.Execute(request);
        return response.IsSuccessful ? $"https://image-mlambda.sirv.com/{directory}/{fileName}" : string.Empty;
      });
    }
  }
}
