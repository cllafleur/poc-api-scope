using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;
using Business.Vacancies;
using System.Dynamic;

namespace WebApp.Adapters
{
    public class VacancyToVacancyModelAdapter : ApiModelAdapterBase<VacancyModel, Vacancy>
    {
        public VacancyToVacancyModelAdapter(IAdapterFactoryService adapterFactoryService, IMappingPredicateProvider propertyMappingResolverProvider)
            : base(adapterFactoryService, propertyMappingResolverProvider)
        {

        }

        public override void Fill(VacancyModel model, Vacancy source)
        {
            ConditionalMap(() => model.Reference = source.Reference, nameof(model.Reference));
            ConditionalMap(() => model.CreationDate = source.CreationDate, nameof(model.CreationDate));
            ConditionalMap(() => model.AgencyComment = source.AgencyComment, nameof(model.AgencyComment));

            ConditionalMap(() => model.JobDescription = this.Fill(model, () => model.JobDescription, source.JobDescription), nameof(model.JobDescription));
        }
    }
}