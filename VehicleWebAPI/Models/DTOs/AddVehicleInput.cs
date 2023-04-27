using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleWebAPI.Models.Enums;

namespace VehicleWebAPI.Models.DTOs
{
    public class AddVehicleInput
    {
        public string VIN { get; set; }
        public VehicleMaker VehicleMaker { get; set; }
        public int VehicleYear { get; set; }
        public string VehicleModel { get; set; }
        public DateTime InspectionDate { get; set; }
        public string InspectorName { get; set; }
        public string InspectionLocation { get; set; }
        public bool PassOrFail { get; set; }
        public string Notes { get; set; }
    }
}