FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY LindDotNetCore.sln /
COPY LindDotNetCore.Api/ LindDotNetCore.Api/
RUN ls
WORKDIR /src/LindDotNetCore.Api
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
RUN ls
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app
RUN ls

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LindDotNetCore.Api.dll"]
