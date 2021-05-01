using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MLambda.Image.Abstract;
using MLambda.Image.fitlers;
using MLambda.Image.health;
using MLambda.Image.Service;

namespace MLambda.Image
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }


    public void ConfigureServices(IServiceCollection services)
    {
      services.AddScoped<IImageService, ImageService>();
      services.AddSingleton<IOptions, Options>();
      services.AddControllers();

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.Authority = "https://oauth.mitienda.co.cr";
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidIssuer = "https://oauth.mitienda.co.cr",
          ValidateIssuerSigningKey = true,
          ValidateAudience = true,
          ValidAudiences = new[] {"swagger"},
          IssuerSigningKey =
            new SymmetricSecurityKey(
              Encoding.ASCII.GetBytes("6368616e676520746869732070617373776f726420746f206120736563726574")),
          ValidateLifetime = true
        };
      });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "MLambda.Image", Version = "v1"});
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
          Type = SecuritySchemeType.OAuth2,
          Flows = new OpenApiOAuthFlows
          {
            AuthorizationCode = new OpenApiOAuthFlow
            {
              AuthorizationUrl = new Uri("https://oauth.mitienda.co.cr/authorize"),
              TokenUrl = new Uri("https://oauth.mitienda.co.cr/token"),
              Scopes = new Dictionary<string, string>
              {
                {"all", "All Scopes"}
              }
            }
          }
        });
        c.OperationFilter<AuthorizeOperationFilter>();
      });
      services.AddHealthChecks().AddCheck<HealthCheck>("image service");
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MLambda.Image v1");


      });
      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("healthz");
      });
    }
  }
}
