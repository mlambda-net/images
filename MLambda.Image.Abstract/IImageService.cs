using System;
using System.IO;
using System.Threading.Tasks;

namespace MLambda.Image.Abstract
{
  public interface IImageService
  {
    IObservable<bool> Delete(string img);
    IObservable<bool> CreateDirectory(string name);
    IObservable<string> Upload(string directory, string fileName, string type, byte[] file);
  }
}
