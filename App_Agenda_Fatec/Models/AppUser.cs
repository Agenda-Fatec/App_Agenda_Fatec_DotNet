using AspNetCore.Identity.MongoDbCore.Models;

using MongoDbGenericRepository.Attributes;

namespace App_Agenda_Fatec.Models
{

    [CollectionName("Users")]
    public class AppUser : MongoIdentityUser // Model padronizada utilizada para cadastrar usuários.
    {

        // Campos personalizados.

        public string? Name { get; set; }

        public bool Administrator { get; set; }

        public bool? Active { get; set; }

    }

}