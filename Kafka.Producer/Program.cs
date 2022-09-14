using Confluent.Kafka;


var publisherConfig = new ProducerConfig
{
    BootstrapServers = "localhost:9092",
    // SslCaLocation = "/Path-to/cluster-ca-certificate.pem",
    // SecurityProtocol = SecurityProtocol.SaslSsl,
    // SaslMechanism = SaslMechanism.ScramSha256,
    // SaslUsername = "ickafka",
    // SaslPassword = "yourpassword",
};

while (true)
{
    Console.WriteLine("Set the message to send:");
    var message = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(message) || message == "exit")
    {
        Console.WriteLine("Exiting...");
        break;
    }

    using (var producer = new ProducerBuilder<string, Msg>(publisherConfig).Build())
    {
        try
        {
            var result = await producer.ProduceAsync("my-topic", new Message<string, Msg> { Key = "my-key", Value = new Msg(message, "@") });
            Console.WriteLine("Message has been produced to topic {0} partition {1} offset {2}", result.Topic, result.Partition, result.Offset);
        }
        catch (ProduceException<string, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Delivery failed: {e.Message}");
        }
    }
}
// var subscriberConfig = new ConsumerConfig
// {
//     BootstrapServers = "localhost:9092",
//     GroupId = "my-group",
//     AutoOffsetReset = AutoOffsetReset.Earliest
// };

// using (var consumer = new ConsumerBuilder<string, string>(subscriberConfig).Build())
// {
//     consumer.Subscribe("my-topic");
//     while (true)
//     {
//         try
//         {
//             var consumeResult = consumer.Consume();
//             Console.WriteLine($"Consumed message in subsriber '{consumeResult.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
//         }
//         catch (ConsumeException e)
//         {
//             Console.WriteLine($"Error occured: {e.Error.Reason}");
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine($"Error occured: {e.Message}");
//         }
//     }
// }

record Msg(string text, string specialChar);