using System;

namespace DeviceManagementWebsite.Models
{
    public class DeviceBackendDTO
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int BackendId { get; set; }
        public string Imei { get; set; }
        public string BackendName { get; set; }
    }
}

