using Xunit;
using Xunit.Abstractions;

namespace xUnit2.Demo.Tests.xUnit.Demo
{
    [Collection("DatabaseCollection")]
    public class SharedContext_CollectionFixture_01
    {
        private DatabaseFixture _dbFixture;
        private ITestOutputHelper _output;
        public SharedContext_CollectionFixture_01(ITestOutputHelper output, DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _output = output;
        }

        [Fact(DisplayName = "SharedContext.CollectionFixture.Case01")]
        public void TestCase01()
        {
            _output.WriteLine("Execute CollectionFixture case 01!");
            _output.WriteLine("DatabaseFixture ExecuteCount is : {0}", DatabaseFixture.ExecuteCount);
        }
    }

    [Collection("DatabaseCollection")]
    public class SharedContext_CollectionFixture_02
    {
        private DatabaseFixture _dbFixture;
        private ITestOutputHelper _output;
        public SharedContext_CollectionFixture_02(DatabaseFixture dbFixture, ITestOutputHelper output)
        {
            _dbFixture = dbFixture;
            _output = output;
        }

        [Fact(DisplayName = "SharedContext.CollectionFixture.Case02")]
        public void TestCase01()
        {
            _output.WriteLine("Execute CollectionFixture case 02!");
            _output.WriteLine("DatabaseFixture ExecuteCount is : {0}", DatabaseFixture.ExecuteCount);
        }
    }

}