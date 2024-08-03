using Amazon.SQS;
using Amazon.SQS.Model;
using MockMentorRESTAPI.Domain.Services;
using System.Net;

namespace MockMentorRESTAPI.Services
{
    public class SQSService : ISQSService
    {
        private readonly IAmazonSQS _sqsClient;

        public SQSService(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }
        
        public async Task<bool> SendMessage(string queueUrl, string messageBody)
        {
            var sendMessageRequest = new SendMessageRequest()
            {
                QueueUrl = queueUrl,
                MessageBody = messageBody
            };
            await _sqsClient.SendMessageAsync(sendMessageRequest);
            return true;
        }

        public async Task<string> GetQueueUrlAsync(string queueName)
        {
            var request = new GetQueueUrlRequest
            {
                QueueName = queueName
            };

            var response = await _sqsClient.GetQueueUrlAsync(request);
            return response.QueueUrl;
        }

        public async Task<List<Message>> ReceiveMessageAsync(string queueUrl)
        {
            int maxNumberOfMessages = 1;
            int waitTimeSeconds = 0;
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = maxNumberOfMessages,
                WaitTimeSeconds = waitTimeSeconds
            };

            var response = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);
            return response.Messages;
        }

        public async Task<bool> DeleteMessageAsync(string queueUrl, string receiptHandle)
        {
            var deleteMessageRequest = new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = receiptHandle
            };

            var response = await _sqsClient.DeleteMessageAsync(deleteMessageRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
