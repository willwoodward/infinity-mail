# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS final
COPY . /source

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages

WORKDIR /source

CMD dotnet watch run --urls http://0.0.0.0:8080