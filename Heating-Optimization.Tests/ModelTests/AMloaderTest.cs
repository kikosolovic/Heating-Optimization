using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heating_Optimization.Models;


namespace Heating_Optimization.Tests.ModelTests
{
    public class AMloaderTest
    {
        [Fact]
        public void AMloader_WhenExecuted_LoadsAMData()
        {
            var data = DataLoader.LoadData<AM>("../../../../Assets/PUData.csv", ";");
            Assert.True(data != null);
        }
    }
}