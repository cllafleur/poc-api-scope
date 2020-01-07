using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Vacancies
{
    public class Vacancy
    {
        public string Reference { get; set; }

        public DateTime CreationDate { get; set; }

        public JobDescription JobDescription { get; set; }

        public string AgencyComment { get; set; }
    }
}
