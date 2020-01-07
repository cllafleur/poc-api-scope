using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Vacancies
{
    public interface IVacancyRepository
    {
        ICollection<Vacancy> GetVacancies();
        Vacancy GetVacancy(int id);
    }
}
