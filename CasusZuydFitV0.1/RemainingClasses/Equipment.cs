using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasusZuydFitV0._1.DAL.DAL;

namespace CasusZuydFitV0._1.RemainingClasses
{
    // Methods gerelateerd aan Equipments zijn (gedeeltelijk) functioneel, maar niet geimplementeerd.
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

        public Equipment()
        {
        }

        static public List<Equipment> GetEquipment()
        {
            EquipmentDAL Dal = new EquipmentDAL();
            Dal.GetEquipment();
            return Dal.equipments;
        }
        public void CreateEquipment()
        {
            EquipmentDAL Dal = new EquipmentDAL();
            Dal.CreateEquipment(this);
        }
        public void UpdateEquipment()
        {
            EquipmentDAL Dal = new EquipmentDAL();
            Dal.UpdateEquipment(this);
        }
        public void DeleteEquipment()
        {
            EquipmentDAL Dal = new EquipmentDAL();
            Dal.DeleteEquipment(this);
        }


    }
}
