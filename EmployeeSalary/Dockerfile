    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /app

    COPY *.sln .
    COPY EmployeeSalary.Servicehost/*.csproj ./EmployeeSalary.Servicehost/
    COPY EmployeeSalary.Application/*.csproj ./EmployeeSalary.Application/
    COPY EmployeeSalary.Domain/*.csproj ./EmployeeSalary.Domain/
    COPY EmployeeSalary.Infrastructure/*.csproj ./EmployeeSalary.Infrastructure/
    COPY EmployeeSalary.Tests/*.csproj ./EmployeeSalary.Tests
    RUN dotnet restore

    COPY EmployeeSalary.Servicehost/. ./EmployeeSalary.Servicehost/
    COPY EmployeeSalary.Application/. ./EmployeeSalary.Application/
    COPY EmployeeSalary.Domain/. ./EmployeeSalary.Domain/
    COPY EmployeeSalary.Infrastructure/. ./EmployeeSalary.Infrastructure/

    RUN dotnet publish EmployeeSalary.Servicehost/EmployeeSalary.Servicehost.csproj -c Release -o /app/publish

    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
    WORKDIR /app

    COPY --from=build /app/publish .

    ENTRYPOINT ["dotnet", "EmployeeSalary.Servicehost.dll"]