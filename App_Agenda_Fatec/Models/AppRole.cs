using AspNetCore.Identity.MongoDbCore.Models;

using MongoDbGenericRepository.Attributes;

namespace App_Agenda_Fatec.Models
{

    [CollectionName("Roles")]
    public class AppRole : MongoIdentityRole // Model padronizada utilizada para cadastrar cargos.
    {



    }

}