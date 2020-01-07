using Business.UseCase;
using System;
using System.Dynamic;

namespace WebApp.Models
{
    [Serializable]
    public class JobDescriptionModel : SerializableModel
    {
        [CustomerUseCases]
        public string ContractCode { get { return _internalObject.ContractCode; } set { _internalObject.ContractCode = value; } }

        [CustomerUseCases]
        [JobBoardUseCase]
        public string JobTitle { get { return _internalObject.JobTitle; } set { _internalObject.JobTitle = value; } }

        [CustomerUseCases]
        [JobBoardUseCase]
        public string Description { get { return _internalObject.Description; } set { _internalObject.Description = value; } }
    }
}