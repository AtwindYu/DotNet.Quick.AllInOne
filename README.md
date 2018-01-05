# DotNet.Test.Demo
dotnet平台下的测试示例


## xUnit.net的相关说明
> 1. 它会并行测试，同一类里的方法不会并行，只有不同类的方法才行并行运行；
> 1. 它有一个测试集合(Test Collections)的概念，不同的Class被看做不同的测试集合。只有不同的测试集合，才会并行运行。
> 1. 通过明确的标记[Collection("Our Test Collection #1")]在类名头上，来表示这是同一测试集合来避免不同的类的并行运行。




## 关于MStest与xUnit 测试的相关说明
> 1. MSTest是不可以有方便的全局变量设定这么一说的，每个类中可以有一个实例级别的初始化与清理。
> 1. MSTest的LiveTest在xUnit中是代码覆盖(可能是Reshpaer中的扩展)测试，MSTest是对号，与红叉，xUnit的覆盖测试是绿条。  


