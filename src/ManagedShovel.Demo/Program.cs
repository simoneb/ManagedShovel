using System;

namespace ManagedShovel.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var shovel = ManagedShovel
                //                          //
                // ------ Mandatory ------- //
                //                          //

                // Sources
                .From("amqp://localhost", "amqp://localhost:5677", "amqp://localhost:5674")

                // Declarations on source
                .Declarations(m => m.ExchangeDeclare("TestExchange", "topic"),
                              m => m.QueueBind(m.QueueDeclare(), "TestExchange", "#"))

                // Destinations
                .To("amqp://localhost/secondary", "amqp://localhost:5675/secondary", "amqp://localhost:5676/secondary")

                // Declarations on destination
                .Declarations(m => m.ExchangeDeclare("TestExchange", "topic"))

                // Shovel queue, either specify one (it needs to exist already) or pick the last created
                //.UseQueue("someQueue")
                .UseLastCreatedQueue()

                //                          //
                // ------- Optional ------- //
                //                          //

                // Prefetch count, 0 (default) means no limit
                .PrefetchCount(50)

                // Ack mode, defaults to OnConfirm
                .AckMode(AckMode.OnConfirm)

                // Publish properties, to change or add to the message properties
                .PublishProperties(p => p.ContentType = "some-content-type")

                // Publish fields, to change exchange and/or routing key of published message
                //.PublishFields("myExchange", "myRoutingKey")

                // Reconnect delay, defaults to 5 seconds
                .ReconnectDelay(TimeSpan.FromSeconds(5))

                // Max hops allowed for the message before it is discarded. Defaults to 1
                .MaxHops(1)

                // Start shovel
                .Start();

            Console.ReadLine();
                         
            shovel.Stop();
        }
    }
}