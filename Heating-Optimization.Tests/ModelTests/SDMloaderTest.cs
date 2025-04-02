using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heating_Optimization.Models;

namespace Heating_Optimization.Tests.ModelTests
{
    public class SDMloaderTest
    {
        [Fact]
        public void SDMloader_WhenExecuted_LoadsSDMData()
        {
            DataLoader.LoadData();
            Assert.True(SDM.SummerPeriod != null && SDM.WinterPeriod != null);
        }
    }
}