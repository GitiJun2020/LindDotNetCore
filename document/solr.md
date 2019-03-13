#### Solr
Solr是在`Lucene`基础之前开发的，使用java编写，一般部署在tomcat上，有自己的图像管理界面，可以用来管理core，
一般地，我们在设计一个core时，需要为它建立对应的实体，与它的core里的属性对应起来；solr有丰富的插件，像一些
中文分词包，索引包等。
```c#
services.AddSolrNet(o =>
{
    o.ServerUrl = "http://192.168.200.214:8081/solr/system_companysubject";
    o.UserName = "sa";
    o.Password = "sa";
});
```