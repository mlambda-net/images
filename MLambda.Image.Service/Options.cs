using System;
using MLambda.Image.Abstract;

namespace MLambda.Image.Service
{
  public class Options : IOptions
  {
    public string Server => Environment.GetEnvironmentVariable("IMAGE_SERVER");

    public string ClientId => Environment.GetEnvironmentVariable("IMAGE_ID");

    public string ClientSecret => Environment.GetEnvironmentVariable("IMAGE_SECRET");
  }
}
