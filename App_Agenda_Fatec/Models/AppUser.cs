using AspNetCore.Identity.MongoDbCore.Models;

using MongoDbGenericRepository.Attributes;

using MongoDB.Bson.Serialization.Attributes;

namespace App_Agenda_Fatec.Models
{

    [CollectionName("Users")]
    public class AppUser : MongoIdentityUser // Model padronizada utilizada para cadastrar usuários.
    {

        public string? Name { get; set; }

        public bool? Active { get; set; }

        [BsonIgnore]
        public string? Activation_Stats { get; set; }

    }

}