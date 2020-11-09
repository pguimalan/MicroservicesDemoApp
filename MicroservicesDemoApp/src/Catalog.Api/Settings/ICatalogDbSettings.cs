namespace Catalog.Api.Settings
{
    public interface ICatalogDbSettings
    {        
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
    }
}
