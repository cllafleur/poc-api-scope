using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Services;

namespace Business.UseCase
{
    public class CustomerUseCasesAttribute : UseCaseAttribute
    {
        public CustomerUseCasesAttribute() : base(UseCases.Customer)
        {
        }
    }
}
