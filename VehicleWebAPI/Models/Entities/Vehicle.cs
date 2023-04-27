using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using VehicleWebAPI.Models.Enums;

namespace VehicleWebAPI.Models.Entities
{
    [Table("Vehicle")]
    public class Vehicle
    {
        [Key]
        [Required]
        [Column(name: "VIN", Order = 0, TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string VIN { get; set; }

        [Required]
        [Column(name: "VehicleMaker", Order = 1)]
        public VehicleMaker VehicleMaker { get; set; }

        [Required]
        [Column(name: "VehicleYear", Order = 2, TypeName = "int")]
        public int VehicleYear { get; set; }

        [Required]
        [Column(name: "VehicleModel", Order = 3, TypeName = "nvarchar(50)")]
        [MaxLength(50)]
        public string VehicleModel { get; set; }

        [Required]
        [Column(name: "InspectionDate", Order = 4, TypeName = "datetime")]
        public DateTime InspectionDate { get; set; }

        [Required]
        [Column(name: "InspectorName", Order = 5, TypeName = "nvarchar(50)")]
        [MaxLength(50)]
        public string InspectorName { get; set; }

        [Required]
        [Column(name: "InspectionLocation", Order = 6, TypeName = "nvarchar(50)")]
        [MaxLength(50)]
        public string InspectionLocation { get; set; }

        [Required]
        [Column(name: "PassOrFail", Order = 7, TypeName = "bit")]
        public bool PassOrFail { get; set; }

        [Required]
        [Column(name: "Notes", Order = 8, TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string Notes { get; set; }
    }
}