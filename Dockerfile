FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MLambda.Image/MLambda.Image.csproj", "MLambda.Image/"]
RUN dotnet restore "MLambda.Image/MLambda.Image.csproj"
COPY . .
WORKDIR "/src/MLambda.Image"
RUN dotnet build "MLambda.Image.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MLambda.Image.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MLambda.Image.dll"]
