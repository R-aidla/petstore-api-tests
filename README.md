# .NET based Petstore API Tests

C# API test project for the [Swagger Petstore API](https://petstore.swagger.io).

## Stack

- [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) — base framework
- [RestSharp](https://restsharp.dev/) — HTTP client
- [xUnit (v2)](https://xunit.net/?tabs=cs) — test framework
- [FluentAssertions](https://fluentassertions.com/) — for cleaner looking tests

## Setup

### Verify that you have .NET 9 installed with
```bash
dotnet --version
```
If the command is not found, [Get the SDK 9.0.15 (9.0.313) from here](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Clone, open project and get all the packages for the project
```bash
git clone https://github.com/R-aidla/petstore-api-tests.git
cd petstore-api-tests
git checkout c#_tests
dotnet restore
```

### To run tests
```bash
dotnet test
```


## What is tested

At least 5 different endpoints:

1. `POST /pet` — create a pet
2. `GET /pet/:id` — (create and) verify the pet exists
3. `PUT /pet` — (create and) update name and status
4. `DELETE /pet/:id` — (create and) delete a pet
5. `GET /pet/findByStatus?status=:status` — get all pet by a status



# Notes

These tests are intended for educational and demonstration purposes against the public Swagger Petstore API
