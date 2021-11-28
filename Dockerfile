FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /source
COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=0 /app .
RUN touch /app/data.txt

CMD [ "./aka" ]