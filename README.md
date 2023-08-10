# Byndyusoft.ApiClient [![Nuget](https://img.shields.io/nuget/v/Byndyusoft.ApiClient.svg)](https://www.nuget.org/packages/Byndyusoft.ApiClient/) [![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.ApiClient.svg)](https://www.nuget.org/packages/Byndyusoft.ApiClient/)
This library simplifies API client creation  

## [BaseClient](https://github.com/Byndyusoft/Byndyusoft.ApiClient/blob/master/src/Byndyusoft.ApiClient/BaseClient.cs) class methods
* GetAsync - retrieves (reads) data via HTTP GET method
* PostAsync - adds (writes) data via HTTP POST method
* PutAsync -  fully updates data via HTTP PUT method
* PatchAsync -  partially updates data via HTTP PATCH method
* DeleteAsync - deletes data via HTTP DELETE method

## Installing
```
dotnet add package Byndyusoft.ApiClient
```

## Usage
To create an API client:
1. Create your API client class and derive it from BaseClient class:
```
public class SomeApiClient : BaseClient
{
	public SomeApiClient(HttpClient client, IOptions<ApiClientSettings> apiSettings) : base(client, apiSettings)
    	{
	}
}
```
2. Great! Now you can use the BaseClient methods to declare your methods:
```
public Task<Model> Create(CreateModelRequest createModelRequest)
	=> PostAsync<Model>("api/create", createModelRequest);
			
public Task Delete(int id) 
	=> DeleteAsync("api/delete/{id}");

public Task<Model> Get(GetModelRequest getModelRequest) =>
	=> GetAsync<GetModelRequest, Model>("api/get, getModelRequest);
```
3.  Make sure to register your client wherever you need it:
```
serviceCollection.AddHttpClient<SomeApiClient>();
```

# Maintainers
[github.maintain@byndyusoft.com](mailto:github.maintain@byndyusoft.com)