using System;

namespace Autopodbor_312.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string NameRu { get; set; }   
        public string BodyRu { get; set; }
        public string NameKy { get; set; }
        public string BodyKy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Publicate { get; set; }
        public string MainImagePath { get; set; }
        public bool IsFieldInspection { get; set; }
        public int CarsBodyTypesId { get; set; }
        public virtual CarsBodyTypes CarsBodyTypes { get; set; }
        public int CarsBrandsId { get; set; }
        public virtual CarsBrands CarsBrands { get; set; }
        public int CarsBrandsModelId { get; set; }
        public virtual CarsBrandsModel CarsBrandsModel { get; set; }

	}
}
