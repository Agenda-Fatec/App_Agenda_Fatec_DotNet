using AspNetCore.Identity.MongoDbCore.Models;

using MongoDbGenericRepository.Attributes;

using MongoDB.Bson.Serialization.Attributes;

namespace App_Agenda_Fatec.Models
{

    [CollectionName("Roles")]
    public class AppRole : MongoIdentityRole // Model padronizada utilizada para cadastrar cargos.
    {

        // Campos personalizados.

        public bool? Active { get; set; }

        [BsonIgnore]
        public string? Activation_Stats { get; set; }

    }

}