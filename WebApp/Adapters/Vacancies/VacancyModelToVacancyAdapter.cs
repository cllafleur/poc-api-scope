using Business.Vacancies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Adapters
{
    public class VacancyModelToVacancyAdapter : ModelAdapterBase<Vacancy, VacancyModel>
    {
        public VacancyModelToVacancyAdapter(IAdapterFactoryService adapterFactoryService, IMappingPredicateProvider propertyMappingResolverProvider)
            : base(adapterFactoryService, propertyMappingResolverProvider)
        {

        }

        public override void Fill(Vacancy model, VacancyModel source)
        {
             ConditionalMap(() => model.Reference = source.Reference, nameof(source.Reference));
             ConditionalMap(() => model.AgencyComment = source.AgencyComment, nameof(source.AgencyComment));
             
             ConditionalMap(() => this.Fill(model, () => model.JobDescription, source.JobDescription), nameof(source.JobDescription));
        }
    }
}