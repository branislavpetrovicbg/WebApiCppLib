#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0.1-nanoserver-20H2 AS base
COPY ./Microsoft.VC143.CRT/ ./Windows/System32/
WORKDIR /app
COPY CppLib.dll .
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SignalRCppLibService/SignalRCppLibService.csproj", "SignalRCppLibService/"]
RUN dotnet restore "SignalRCppLibService/SignalRCppLibService.csproj"
COPY . .
WORKDIR "/src/SignalRCppLibService"
RUN dotnet build "SignalRCppLibService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SignalRCppLibService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalRCppLibService.dll"]