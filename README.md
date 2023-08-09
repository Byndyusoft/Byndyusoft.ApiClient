# API client creation library  [![Nuget](https://img.shields.io/nuget/v/Byndyusoft.ApiClient.svg)](https://www.nuget.org/packages/Byndyusoft.ApiClient/) [![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.ApiClient.svg)](https://www.nuget.org/packages/Byndyusoft.ApiClient/)

## Methods of BaseClient
* GetAsync - used for retrieving (reading)
* PostAsync - used for adding (writing)
* PutAsync - used for full update
* PatchAsync - used for partial update
* DeleteAsync - used for deletion

## Installing
```
dotnet add package Byndyusoft.ApiClient
```

## Usage
To create an API client:
1. Create a class for your API client and make it derived from BaseClient:
```
public class SomeApiClient : BaseClient
{
	public SomeApiClient(HttpClient client, IOptions<ApiClientSettings> apiSettings) : base(client, apiSettings)
    	{
	}
}
```
2. Great! Now you can use the predefined methods to describe your HTTP requests:
```
public Task<Model> Create(CreateModelRequest createModelRequest)
	=> PostAsync<Model>("api/create", createModelRequest);
			
public Task Delete(int id) 
	=> DeleteAsync("api/delete/{id}");

public Task<Model> Get(GetModelRequest getModelRequest) =>
	=> GetAsync<GetModelRequest, Model>("api/get, getModelRequest);
```
3.  Make sure to include your client wherever you need to use it:
```
serviceCollection.AddHttpClient<SomeApiClient>();
```

# Maintainers
[github.maintain@byndyusoft.com](mailto:github.maintain@byndyusoft.com)