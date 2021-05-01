using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MLambda.Image.health
{
  public class HealthCheck: IHealthCheck
  {
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
      return HealthCheckResult.Healthy("Image Server");
    }
  }
}
