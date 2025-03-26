namespace Agenda.Domain.Entities

{
    public class Contato
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string TipoTelefone { get; set; }

        public Contato(string nome, string email, string telefone, string tipoTelefone)
        {
            Nome = nome;
            Email = email;
            Telefone = telefone;
            TipoTelefone = tipoTelefone;
        }
    }
}
