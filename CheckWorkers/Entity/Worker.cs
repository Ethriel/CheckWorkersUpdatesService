using System;

namespace CheckWorkers.Entity
{
    public class Worker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public DateTime TimeUpdated { get; set; }
        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }
    }
}
