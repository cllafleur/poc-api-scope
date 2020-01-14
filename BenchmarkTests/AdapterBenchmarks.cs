using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Adapters;
using WebApp.Tests.Adapters;

namespace BenchmarkTests
{
    public class AdapterBenchmarks
    {
        [Benchmark]
        public void AdapterExpressionWithoutExceptionBench()
        {
            var tests = new JobDescriptionModelToJobDescriptionAdapter2Tests();
            tests.Setup();
            tests.Fill_UseCaseCustomer_ReturnsModel2();
            tests.Fill_UseCaseCustomer_ReturnsModel2();
            tests.Fill_UseCaseCustomer_ReturnsModel2();
        }

        [Benchmark]
        public void AdapterExpressionWithExceptionBench()
        {
            var tests = new JobDescriptionModelToJobDescriptionAdapter2Tests();
            tests.Setup();
            tests.Fill_UseCaseCustomer_ContractThrowsException_ReturnsModel2();
            tests.Fill_UseCaseCustomer_ContractThrowsException_ReturnsModel2();
            tests.Fill_UseCaseCustomer_ContractThrowsException_ReturnsModel2();
        }


        [Benchmark]
        public void AdapterLitteralWithExceptionBench()
        {
            var tests = new JobDescriptionModelToJobDescriptionAdapterTests();
            tests.Setup();
            tests.Fill_UseCaseCustomer_ContractThrowsException_ReturnsModel();
            tests.Fill_UseCaseCustomer_ContractThrowsException_ReturnsModel();
            tests.Fill_UseCaseCustomer_ContractThrowsException_ReturnsModel();
        }

        [Benchmark]
        public void AdapterLitteralWithoutExceptionBench()
        {
            var tests = new JobDescriptionModelToJobDescriptionAdapterTests();
            tests.Setup();
            tests.Fill_UseCaseCustomer_ReturnsModel();
            tests.Fill_UseCaseCustomer_ReturnsModel();
            tests.Fill_UseCaseCustomer_ReturnsModel();
        }
    }
}
