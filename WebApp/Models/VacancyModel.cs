using Business.UseCase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    [Serializable]
    public class VacancyModel : SerializableModel
    {
        [CustomerUseCases]
        [JobBoardUseCase]
        [JsonProperty("reference")]
        public string Reference { get { return _internalObject.Reference; } set { _internalObject.Reference = value; } }

        [CustomerUseCases]
        [JobBoardUseCase]
        [JsonProperty("creationDate")]
        public DateTime? CreationDate { get { return _internalObject.CreationDate; } set { _internalObject.CreationDate = value; } }

        [CustomerUseCases]
        [JobBoardUseCase]
        [JsonProperty("description")]
        public JobDescriptionModel JobDescription { get { return _internalObject.JobDescription; } set { _internalObject.JobDescription = value; } }

        [CustomerUseCases]
        [JsonProperty("agencyComment")]
        public string AgencyComment { get { return _internalObject.AgencyComment; } set { _internalObject.AgencyComment = value; } }
    }
}