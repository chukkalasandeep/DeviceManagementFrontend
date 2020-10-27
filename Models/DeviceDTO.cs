using System;

namespace DeviceManagementWebsite.Models
{
    public class DeviceDTO
    {
        public int Id { get; set; }
        public string Imei { get; set; }
        public decimal SimCardNumber { get; set; }
        public bool Enabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
    }
}

