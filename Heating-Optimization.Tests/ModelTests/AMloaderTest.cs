using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heating_Optimization.Models;
using Xunit;


namespace Heating_Optimization.Tests.ModelTests
{
    public class AMloaderTest
    {
        [Fact]
        public void AMloader_WhenExecuted_LoadsAMData()
        {
            DataLoader.LoadData();
            Assert.True(AM.ProductionUnits != null);
        }
    }
}