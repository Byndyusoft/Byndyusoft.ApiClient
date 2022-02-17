Билиотека упрощает создание клиента для api

Для создания клиента к api:
1. Создайте класс клиента к api и сделаейте его производным от BaseClient
```
public class SomeApiClient : BaseClient
{
	public SomeApiClient(HttpClient client, IOptions<ApiClientSettings> apiSettings) : base(client, apiSettings)
    {
	}
}
```
2. Чудесно! Теперь вы можете использовать готовые методы для описания http-запросов
```
	public Task<Model> Create(CreateModelRequest createModelRequest)
		=> PostAsync<Model>("api/create", createModelRequest);
			
    public Task Delete(int id) 
		=> DeleteAsync("api/delete/{id}");

    public Task<Model> Get(GetModelRequest getModelRequest) =>
		=> GetAsync<GetModelRequest, Model>("api/get, getModelRequest);
```
3. Не забываем подключить ваш клиент
```
    serviceCollection.AddHttpClient<SomeApiClient>();
```


