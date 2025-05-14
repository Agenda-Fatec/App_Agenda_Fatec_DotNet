using MongoDB.Driver;

namespace App_Agenda_Fatec.Models
{

    public class MongoDBContext
    {

        // Atributos de conexão com o banco de dados.

        public static string? Connection_String { get; set; }

        public static string? Database_Name { get; set; }

        public static bool Is_Ssl { get; set; }

        // Atributo de acesso ao banco de dados.

        private IMongoDatabase Database { get; }

        // Método construtor da classe.

        public MongoDBContext()
        {

            try
            {

                MongoClientSettings connection_settings = MongoClientSettings.FromUrl(new MongoUrl(Connection_String));

                if (Is_Ssl)
                {

                    connection_settings.SslSettings = new SslSettings()
                    {

                        EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12

                    };

                }

                MongoClient connection = new MongoClient(connection_settings);

                this.Database = connection.GetDatabase(Database_Name);

            }

            catch (Exception ex)
            {

                throw new Exception("Não foi possível se conectar ao MongoDB.", ex);

            }

        }

        // Coleções do banco de dados (MongoDB).

        public IMongoCollection<Block> Blocks { get { return this.Database.GetCollection<Block>("Blocks"); } }

    }

}