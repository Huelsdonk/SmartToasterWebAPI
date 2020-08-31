namespace ToastApiReact
{
    public class ToastersDatabaseSettings : IToastersDatabaseSettings
    {
        public string ToastersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IToastersDatabaseSettings
    {
        string ToastersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}


// this sets up the name of the collection, the name of the DB, and the
// connection port to connect to the DB. They're all stored in the app
// settings