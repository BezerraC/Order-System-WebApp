namespace ArkdBarV1.Entities
{
    public class EmailConfiguration
    {
        public string NomeRemetente { get; set; }

        public string EmailRemetente { get; set; }

        public string Senha { get; set; }

        public string EnderecoServidorEmail { get; set; }

        public string PortaServidorEmail { get; set; }

        public bool UsarSsl { get; set; }
    }
}
