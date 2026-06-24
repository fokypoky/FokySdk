# FokySdk
Инфраструктурный SDK для .NET микросервисов. Содержит подключение Swagger, OpenTelemetry, RabbitMQ, Entity Framework Core и прочего
# Упаковываем в NuGet пакет
* Собираем проект в конфигурации Release
```sh
dotnet build -c Release
```
* Собираем пакет. Вызываем из корня проекта:
```sh
dotnet pack ./FokySdk/FokySdk.csproj -c Release -o ./nupkgs
```