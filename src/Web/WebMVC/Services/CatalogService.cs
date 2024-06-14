using Catalog.API.Messages.Request;
using Catalog.API.Messages.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace WebMVC.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettings _appSettings;

        public CatalogService(IHttpClientFactory httpClientFactory, AppSettings appSettings)
        {
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings;
        }

        public async Task CreatePlate(CreatePlateRequest createPlateRequest)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post, 
                $"/api/Plate/?Registration={createPlateRequest.Registration}&PurchasePrice={createPlateRequest.PurchasePrice}&SalePrice={createPlateRequest.SalePrice}");
            var httpClient = _httpClientFactory.CreateClient("CatalogApi");
            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            }
            catch (Exception e)
            {
                throw new Exception($"Problem connecting to Catalog API {e}");
            }

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                try
                {
                    return;
                }
                catch (JsonSerializationException e)
                {
                    throw new Exception($"Problem deserializing response {e}");
                }
            }

            throw new Exception($"Api call unsuccessful. Http code {httpResponseMessage.StatusCode}");
        }

        public async Task<GetPlateItemsResult> GetPlateItems(GetPlateItemsRequest getPlateItemsRequest)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/Plate/GetPlateItems?PageNumber={getPlateItemsRequest.PageNumber}&SortOrder={getPlateItemsRequest.SortOrder}");
            var httpClient = _httpClientFactory.CreateClient("CatalogApi");

            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            }
            catch (Exception e)
            {
                throw new Exception($"Problem connecting to Catalog API {e}");
            }

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                try
                {
                    string apiResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GetPlateItemsResult>(apiResponse) ?? new GetPlateItemsResult();
                }
                catch (JsonSerializationException e)
                {
                    throw new Exception($"Problem deserializing response {e}");
                }
            }

            throw new Exception($"Api call unsuccessful. Http code {httpResponseMessage.StatusCode}");
        }
    }
}
