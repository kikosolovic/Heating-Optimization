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
            var data = DataLoader.LoadData<SDM>("../../../../Assets/SDMData.csv", ",");
            Assert.True(data.Item1 != null && data.Item2 != null);
        }
    }
}