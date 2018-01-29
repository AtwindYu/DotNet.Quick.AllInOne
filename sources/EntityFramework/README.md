# EntityFramework.Demo
EntityFramework 6 及 Core的相关学习示例


## EF6 的数据迁移功能
> 1. 首先是使用DbContext来建立数据库表。（如果你的数据库以后再也不会改变的话，那么这个就够了。不用什么迁移功能）；     
> 1. 如果你的运行项目根目录下没有迁移目录``Migrations`` 那就要先要在项目的Nuget控制台中执行命令： ``Enable-Migrations`` 来``打开迁移功能``（命令执行后，会检测当前使用的Db，并且增加迁移目录：``Migrations``，这是迁移的第一个节点 ）；     
> 1. 现在我们来改变一下，原本数据模型的属性（随便增减）；     
> 1. 现在必须要``增加迁移节点``，才能正确运行。Add-Migration EF6.Demo(迁移项目名称) ，它就会自动侦测相应的变更。注意，此时还是不能直接执行哟；     
> 1. 现在必须要``更新迁移节点内容到Db中去`` ，还是在Nuget的控制台中执行：``Update-Database`` 命令，如果要查看升级的详情可以用``Update-Database -Verbose``；     

#### PS:EF6 的数据迁移功能的相关说明   
> 1. Migrations目录下的Configuration.Seed方法，这个就是让你主动去调用AddOrUpdate来更新原来旧的版本数据在版本里的内容的。         
> 1. 每次更新数据结构要进行迁移时就重复执行Add-Migration 和Update-Database。 再次Add-Migration时会要求名称 ``Add-Migration WhatRUDid`` 。        
> 1. 改属性的时候会发现，原来的属性上的数据都没呢。如果想保留原来的数据，那么在执行了Add-Migration 后修改增加的类，将AddColumn和DropColumn注掉，增加``RenameColumn("dbo.User", "Address2","Address");``，然后执行Update-Database。就可以保留原来的字段里的内容了。
> 1. 在数据类型的自动映射中有些不是我们想要的。比如string->nvarchar(max) 我们想要text, DateTime类型我们只想要Date数据，这时就要手动配置了，参见：``EF6.Demo.DbAccess.UserConfiguration``的构造方法。    


---------------------------------------

## EF.Core 的数据迁移功能，采用MySql数据库
EF.Core改变比较多，和原来的内容有很多不一样的地方。[参考文档](https://docs.microsoft.com/zh-cn/ef/core/)            

> 1. 项目中增加Nuget包：``Microsoft.EntityFrameworkCore.Tools`` ；     
> 1. 在vs的Nuget控制台，输入以下命令``Add-Migration init`` (注意：不能有多个EF类型的项目同时运行。)
> 1. 输入命令：``Update-Database init`` 生成数据库  （不用事先建立数据库，会帮你生成的）
> 1. 移除一个属性，然后执行 ``add-migration rmovetimestap`` 会生成新的迁移代码。
> 1. 执行 ``update-database`` 提交至Db库中。
> 1. 执行``remove-migration thecommitname``来回滚迁移。 

#### PS:EF.Core 的数据迁移功能的相关说明
> 1. 涉及到主键变更的不要使用迁移命令，手动修改吧。 
> 1. 变更主键好像会失败，即表一旦设定了主键就不能通过迁移功能来修改，要手动修改成的迁移代码，将PK_TableName那一块代码给注掉。 
> 1. 更新迁移时多使用``update-database -verbose``命令来查看发生的错误并进行手动修正。目前在Mysql上确实发现迁移并不是那么自动化。   
> 1. 在Mysql下字段的顺序不是类的顺序，而是按字母排序。
> 1. 如果你修改了某个字段从可空为必须，那么原来的表中一定要填满内容，否则出错。





--------------------------------

## Entity Framework 迁移命令（get-help EntityFramework）
> ``Enable-Migrations`` 启用迁移            
> ``Add-Migration`` 为挂起的Model变化添加迁移脚本           
> ``Update-Database`` 将挂起的迁移更新到数据库           
> ``Get-Migrations`` 获取已经应用的迁移           
运行``Update-Database`` 来升级数据库到最新版本. 我们可以通过指定 ``–Verbose`` 看到 SQL的执行情况.

> . 在 Package Manager Console运行 ``Update-Database –Verbose`` .   
> . ``将数据库更新到指定的版本Migration（包括升级 ,降级或者说是回滚）``
到目前为止我们已经可以将数据库更新到最新版本Migration,但有时需要升级或降级到指定的Migration.
>> 现在有个特殊情况想让数据库回到运行完``AddBlogUrl migration``的状态。这事我们就可以使用``–TargetMigration`` 来降级数据库到该版本

>> 在Package Manager Console 运行Update-Database –TargetMigration: AddBlogUrl 命令.           
>> 该命令将会运行  AddBlogAbstract 和AddPostClass migration的 Down脚本 .          
>> 如果想回到最初 Update-Database –TargetMigration: $InitialDatabase 这条命令将会帮你一步到位.          





