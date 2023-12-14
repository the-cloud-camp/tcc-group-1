#dotnet 7 docker multistate build
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o output
# Test 


# Serve stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS serve
WORKDIR /app

# For Alpine Image
#RUN apk add --no-cache icu-libs
#ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

#RUN apk add --no-cache tzdata
#ENV TZ=Asia/Bangkok
#RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN addgroup --system --gid 1400 bojappG && adduser --system --uid 1400 --ingroup bojappG --shell /bin/sh bojapp
RUN chown -Rf bojapp:bojappG /app && chmod -Rf 760 /app

USER bojapp

COPY --from=build /src/output .

#For NET 6-7 
#Default NET8 is 8080
ENV ASPNETCORE_URLS=http://+:8090

#ASPNETCORE_HTTPS_PORT Required Certfile
#ENV ASPNETCORE_URLS=https://+:8091;http://+:8090
#ENV ASPNETCORE_HTTPS_PORT=8091

EXPOSE 8090

ENTRYPOINT [ "dotnet", "bojpawnapi.dll" ]
