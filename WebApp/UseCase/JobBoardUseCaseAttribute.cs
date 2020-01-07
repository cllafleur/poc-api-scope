using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Services;

namespace Business.UseCase
{
    public class JobBoardUseCaseAttribute : UseCaseAttribute
    {
        public JobBoardUseCaseAttribute() : base(UseCases.JobBoard)
        {
        }
    }
}
