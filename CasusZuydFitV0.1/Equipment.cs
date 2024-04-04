using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasusZuydFitV0._1
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentDescription { get; set; }
        public bool EquipmentAvailability { get; set; }

        public Equipment(int equipmentId, string equipmentName, string equipmentDescription, bool equipmentAvailability)
        {
            EquipmentId = equipmentId;
            EquipmentName = equipmentName;
            EquipmentDescription = equipmentDescription;
            EquipmentAvailability = equipmentAvailability;
        }
    }
}
