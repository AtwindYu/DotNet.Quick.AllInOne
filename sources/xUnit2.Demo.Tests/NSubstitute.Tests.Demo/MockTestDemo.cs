using Xunit;

namespace Sunkong.CheckingInSystem.Test.NSubstitute.Tests.Demo
{

    

    public class MockTestDemo
    {



        [Fact]
        public void Run1()
        {
            Assert.True(1==1);
        }


        //[Fact]
        //public void Test_Create_User()
        //{
        //    var userRepository = MockRepository.GenerateMock<IRepository<User>>();
        //    userRepository.Expect(ur => ur.Insert(Arg<User>.Is.Anything));
        //    var userService = new UserService(userRepository);
        //    userService.Create(new User() { UserName = "zhangsan" });
        //    userRepository.VerifyAllExpectations();
        //}
    }
}