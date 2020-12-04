using System.Collections.Generic;

namespace CheckWorkers.Entity
{
    public class Company
    {
        public Company()
        {
            Workers = new HashSet<Worker>();
        }

        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Worker> Workers { get; set; }
    }
}
