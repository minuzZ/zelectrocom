using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZelectroCom.Web.Infrastructure;

namespace ZelectroCom.Test
{
    [TestClass]
    public class CommonTests
    {
        [TestMethod]
        public void AutoMapperConfig_Valid()
        {
            AutoMapperConfiguration.Configure();
            Mapper.AssertConfigurationIsValid();
        }
    }
}
