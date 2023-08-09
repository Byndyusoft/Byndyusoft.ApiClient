# Library of client creation for API [![Nuget](https://img.shields.io/nuget/v/Byndyusoft.ApiClient.svg)](https://www.nuget.org/packages/Byndyusoft.ApiClient/) [![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.ApiClient.svg)](https://www.nuget.org/packages/Byndyusoft.ApiClient/)

## Methods of BaseClient
* GetAsync - is used to receive (read)
* PostAsync - is used to add (record)
* PutAsync - is used for a full update
* PatchAsync - is used for partial update
* DeleteAsync - is used to remove

## Installing
```
dotnet add package Byndyusoft.ApiClient
```

## Usage
To create a client for api:
1. Create a client class to api and make it derived from BaseClient

```
public class SomeApiClient : BaseClient
{
	public SomeApiClient(HttpClient client, IOptions<ApiClientSettings> apiSettings) : base(client, apiSettings)
    	{
	}
}
```
2. Now you can use ready-made methods to describe http requests
```
public Task<Model> Create(CreateModelRequest createModelRequest)
	=> PostAsync<Model>("api/create", createModelRequest);
			
public Task Delete(int id) 
	=> DeleteAsync("api/delete/{id}");

public Task<Model> Get(GetModelRequest getModelRequest) =>
	=> GetAsync<GetModelRequest, Model>("api/get, getModelRequest);
```
3. Don't forget to connect your client
```
    serviceCollection.AddHttpClient<SomeApiClient>();
```

# Maintainers
[github.maintain@byndyusoft.com](mailto:github.maintain@byndyusoft.com)