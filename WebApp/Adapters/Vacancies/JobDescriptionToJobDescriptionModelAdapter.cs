using Business.Services;
using Business.Vacancies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Adapters
{
    public class JobDescriptionToJobDescriptionModelAdapter : ApiModelAdapterBase<JobDescriptionModel, JobDescription>
    {
        private readonly IIdToCodeConverterService _idConverterService;

        public JobDescriptionToJobDescriptionModelAdapter(IAdapterFactoryService adapterFactoryService, IMappingPredicateProvider propertyMappingResolverProvider, IIdToCodeConverterService idConverterService)
            : base(adapterFactoryService, propertyMappingResolverProvider)
        {
            _idConverterService = idConverterService;
        }

        public override void Fill(JobDescriptionModel model, JobDescription source)
        {
            ConditionalMap(() => model.ContractCode = _idConverterService.GetCode(source.ContractId), nameof(model.ContractCode));
            ConditionalMap(() => model.JobTitle = source.JobTitle, nameof(model.JobTitle));
            ConditionalMap(() => model.Description = source.Description, nameof(model.Description));
        }
    }
}