using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace xUnit2.Demo.Tests.xUnit.Demo
{
    public class EqualExample
    {
        private readonly ITestOutputHelper _output;

        public EqualExample(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void EqualStringIgnoreCase()
        {
            string expected = "TestString";
            string actual = "teststring";

            Assert.False(actual == expected);
            Assert.NotEqual(expected, actual);
            Assert.Equal(expected, actual, StringComparer.CurrentCultureIgnoreCase);
        }


        [Fact(DisplayName = "Lesson02.Demo01")]
        public void Demo01_Fact_Test()
        {
            int num01 = 1;
            int num02 = 2;
            Assert.Equal<int>(3, num01 + num02);
        }

        [Fact(DisplayName = "Lesson02.Demo02", Skip = "Just test skip!")]
        public void Demo02_Fact_Test()
        {
            int num01 = 1;
            int num02 = 2;
            Assert.Equal<int>(3, num01 + num02);
        }

        [Fact]
        public void YearsTest()
        {
            var t1 = new DateTime(1900,1,1);
            var t2 = DateTime.Now;
            var t = t2 - t1;

            _output.WriteLine((t.TotalDays/365).ToString());


            _output.WriteLine(ToUnixTimeStamp(DateTime.Now.AddYears(-16)));


        }

        private string ToUnixTimeStamp(DateTime? dateTime)
        {
            if (dateTime == null) return null;
            return ((dateTime.Value.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
        }

        #region 数据驱动测试 DEMO


        #region MemberData InputData_Property  下面的Code中定义了属性 InputData_Property，并在测试方法上用MemberData标记说明数据源来自对应的属性。

        public static IEnumerable<object[]> InputData_Property
        {
            get
            {
                var driverData = new List<object[]>();
                driverData.Add(new object[] { 1, 1, 2 });
                driverData.Add(new object[] { 1, 2, 3 });
                driverData.Add(new object[] { 2, 3, 5 });
                driverData.Add(new object[] { 3, 4, 7 });
                driverData.Add(new object[] { 4, 5, 9 });
                driverData.Add(new object[] { 5, 6, 11 });
                return driverData;
            }
        }

        [Theory(DisplayName = "Lesson02.Demo04")]
        [MemberData(nameof(InputData_Property))]
        public void Demo04_Theory_Test(int num01, int num02, int result)
        {
            Assert.Equal<int>(result, num01 + num02);
        }

        #endregion


        #region MemberData InputData_Method   多了一个flag参数。这个参数的值从何而来呢？就是我们之前说的MemberData属性的第二个构造参数

        public static IEnumerable<object[]> InputData_Method(string flag)
        {
            var driverData = new List<object[]>();
            if (flag == "Default")
            {
                driverData.Add(new object[] { 1, 1, 2 });
                driverData.Add(new object[] { 1, 2, 3 });
                driverData.Add(new object[] { 2, 3, 5 });
            }
            else
            {
                driverData.Add(new object[] { 3, 4, 7 });
                driverData.Add(new object[] { 4, 5, 9 });
                driverData.Add(new object[] { 5, 6, 11 });
            }
            return driverData;
        }

        [Theory(DisplayName = "Lesson02.Demo05")]
        [MemberData(nameof(InputData_Method), "Default")]
        [MemberData(nameof(InputData_Method), "Other")]
        public void Demo05_Theory_Test(int num01, int num02, int result)
        {
            Assert.Equal<int>(result, num01 + num02);
        }

        #endregion MemberData InputData_Method


        #region MemberData InputData_Field 这里使用输入的两个数据集合做笛卡尔积的结果，来充当数据源

        //MatrixData字段在构造的时候就会按照规则（使用Numbers，Strings的笛卡尔积）构造对应的数据源。
        //看一下Test Explorer视图，此方法对应了6（3×2 = 6）个用例，用例的参数就是两个数组的笛卡尔积的组合：

        public static int[] Numbers = { 5, 6, 7 };
        public static string[] Strings = { "Hello", "world!" };
        public static MatrixTheoryData<string, int> MatrixData = new MatrixTheoryData<string, int>(Strings, Numbers);

        [Theory(DisplayName = "Lesson02.Demo06")]
        [MemberData(nameof(MatrixData))]
        public void Demo06_Theory_Test(string x, int y)
        {
            Assert.Equal(y, x.Length);
        }

        #endregion MemberData InputData_Field

        #endregion


    }
}