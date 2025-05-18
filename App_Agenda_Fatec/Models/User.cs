using System.ComponentModel.DataAnnotations;

using MongoDB.Bson.Serialization.Attributes;

using System.Globalization;

using System.Text;

namespace App_Agenda_Fatec.Models
{

    public class User
    {

        public Guid _id;

        public Guid Id { get { return this._id; } set { this._id = value; } }

        [Required]
        [Display(Name = "Nome")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Telefone | Celular")]
        [Phone(ErrorMessage = "Telefone inválido.")]
        public string? Phone { get; set; }

        [Required]
        public string? Password { get; set; }

        [BsonIgnore]
        public bool Administrator { get; set; } = false; // Valor padrão.

        public bool? Active { get; set; } = true; // Valor padrão.

        [BsonIgnore]
        [Display(Name = "Status")]
        public string? Activation_Stats { get; set; }

        [BsonIgnore]
        [Display(Name = "Cargos")]
        public string[]? Roles { get; set; }

        // Aprofundamento: https://gist.github.com/PabloValentin94/67343c258863eb1d157b881bf5adb074

        public static string Remove_Accents(string user_name)
        {

            string raw_text = user_name.Normalize(NormalizationForm.FormD);

            StringBuilder formatted_string = new StringBuilder();

            foreach (char character in raw_text)
            {

                if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                {

                    formatted_string.Append(character);

                }

            }

            return formatted_string.ToString().Normalize(NormalizationForm.FormC);

        }

    }

}