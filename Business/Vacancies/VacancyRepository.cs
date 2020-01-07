using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Vacancies
{
    public class VacancyRepository : IVacancyRepository
    {
        private Vacancy[] vacancies = new[] {
        new Vacancy(){CreationDate = DateTime.Now, Reference = "2019-1", AgencyComment= "Super comment", JobDescription = new JobDescription(){ ContractId=1, JobTitle="Offer 1", Description="Come !!!" } },
        new Vacancy(){CreationDate = DateTime.Now, Reference = "2019-2", AgencyComment= "Super comment", JobDescription = new JobDescription(){ ContractId=1, JobTitle="Offer 2", Description="Come !!!" } },
        new Vacancy(){CreationDate = DateTime.Now, Reference = "2019-3", AgencyComment= "Super comment", JobDescription = new JobDescription(){ ContractId=1, JobTitle="Offer 3", Description="Come !!!" } },
        };

        public ICollection<Vacancy> GetVacancies()
        {
            return vacancies;
        }

        public Vacancy GetVacancy(int id)
        {
            if (id > vacancies.Length)
                return null;
            return vacancies[id - 1];
        }
    }
}
