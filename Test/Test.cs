using System.Threading.Tasks;
using BusinessLogic.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public async Task TestMethod()
        {
            await SchedulerLogic.Compute();
        }
    }
}
