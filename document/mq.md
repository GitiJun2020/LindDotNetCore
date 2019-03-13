#### 消息队列
消息队列主要使用'rabbitmq,kafka'实现的，用来解耦项目，处理高并发任务和耗时任务，生产者
不需要关心是谁来消费，它只管把消息发到队列中；而消费者不关心消息如何产生，只把消费按着
业务逻辑去处理掉！
```c#
services.AddRabbitMQ(o =>
{
    o.ExchangeName = "Piliapa.zzl";
    o.MqServerHost = "192.168.200.214";
    o.VirtualHost = "/";
    o.ExchangeType = "topic";
});
```