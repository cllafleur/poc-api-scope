using Business.Services;
using Business.Vacancies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Adapters
{
    public class JobDescriptionModelToJobDescriptionAdapter2 : ModelAdapter2Base<JobDescription, JobDescriptionModel>
    {
        private readonly IIdToCodeConverterService _idConverterService;

        public JobDescriptionModelToJobDescriptionAdapter2(IAdapterFactoryService adapterFactoryService, IMappingPredicateProvider propertyMappingResolverProvider, IIdToCodeConverterService idConverterService)
            : base(adapterFactoryService, propertyMappingResolverProvider)
        {
            _idConverterService = idConverterService;
        }

        public override void Fill(JobDescription model, JobDescriptionModel source)
        {
            ConditionalMap(() => model.ContractId = GetValue(_idConverterService.GetId, source.ContractCode, () => model.ContractId) ?? model.ContractId, nameof(source.ContractCode));
            ConditionalMap(() => model.JobTitle = source.JobTitle, nameof(source.JobTitle));
            ConditionalMap(() => model.Description = source.Description, nameof(source.Description));
        }
    }
}