namespace Autopodbor_312.Models
{
    public class Services
    {
        public int Id { get; set; }
        #nullable enable
		public string? NameRu { get; set; }
        public string? DescriptionRu { get; set; }
        public string? NameKy { get; set; }
        public string? DescriptionKy { get; set; }
        public bool? IsAdditional { get; set; }
        public string? Photo { get; set; }
	}
}
