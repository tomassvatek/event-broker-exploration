using Confluent.Kafka;

namespace Kafka.Producer;

public interface IMessageBroker
{
    Task PublishAsync(string message);
}

record Msg(string text, string specialChar);

public class MessageBroker : IMessageBroker
{
    public async Task PublishAsync(string message)
    {
        var publisherConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            // SslCaLocation = "/Path-to/cluster-ca-certificate.pem",
            // SecurityProtocol = SecurityProtocol.SaslSsl,
            // SaslMechanism = SaslMechanism.ScramSha256,
            // SaslUsername = "ickafka",
            // SaslPassword = "yourpassword",
        };

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
}