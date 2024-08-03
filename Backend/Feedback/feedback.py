import os
import json
import openai
import boto3

openai.api_key = os.getenv('OPENAI_API_KEY')

region_name = os.getenv('AWS_DEFAULT_REGION')
aws_access_key_id = os.getenv('AWS_ACCESS_KEY_ID')
aws_secret_access_key = os.getenv('AWS_SECRET_ACCESS_KEY')
aws_session_token = os.getenv('AWS_SESSION_TOKEN')

sqs = boto3.client(
    'sqs',
    region_name=region_name,
    aws_access_key_id=aws_access_key_id,
    aws_secret_access_key=aws_secret_access_key,
    aws_session_token=aws_session_token
)

def get_chatgpt_feedback(question_text, user_answer):
    print(f"Generating feedback for the question: {question_text}")
    messages = [
        {"role": "system", "content": "You are an expert in providing detailed feedback on answers. Provide only feedback, not a review or an improved answer."},
        {"role": "user", "content": f"Question: {question_text}\nAnswer: {user_answer}\n\nReview the answer thoroughly and provide detailed feedback on how to improve the answer."}
    ]

    try:
        response = openai.ChatCompletion.create(
            model="gpt-3.5-turbo",
            messages=messages,
            max_tokens=300
        )
        feedback = response.choices[0].message['content'].strip()
        print(f"Feedback received: {feedback}")
        return feedback
    except Exception as e:
        print(f"Error calling ChatGPT API: {e}")
        return "Error generating feedback."

def create_queue_if_not_exists(queue_name):
    response = sqs.list_queues(QueueNamePrefix=queue_name)
    if 'QueueUrls' in response:
        for url in response['QueueUrls']:
            if queue_name in url:
                return url

    response = sqs.create_queue(QueueName=queue_name)
    return response['QueueUrl']

def process_message(message, response_queue_url):
    
    try:
        body = json.loads(message['Body'])
        question_text = body.get('questionText')
        answerId = body.get('answerId')
        user_answer = body.get('answerText')
        messageType = body.get('messageType')

        if not question_text or not user_answer:
            print("Missing 'questionText' or 'answerText' in message body.")
            return

        feedback = get_chatgpt_feedback(question_text, user_answer)

        response_message = {
            'questionText': question_text,
            'answerText': user_answer,
            'feedbackText': feedback,
            'messageType': messageType,
            'answerId': answerId
        }

        try:
            print(f"Sending response message to queue: {response_queue_url}")
            sqs.send_message(
                QueueUrl=response_queue_url,
                MessageBody=json.dumps(response_message)
            )
        except Exception as e:
            print(f"Error sending message to SQS: {e}")
    except json.JSONDecodeError as e:
        print(f"Error decoding JSON from message body: {e}")

def listen_for_messages(request_queue_name, response_queue_name):
    request_queue_url = create_queue_if_not_exists(request_queue_name)
    response_queue_url = create_queue_if_not_exists(response_queue_name)

    while True:
        response = sqs.receive_message(
            QueueUrl=request_queue_url,
            AttributeNames=['All'],
            MaxNumberOfMessages=1,
            WaitTimeSeconds=20
        )

        if 'Messages' in response:
            for message in response['Messages']:
                process_message(message, response_queue_url)
                sqs.delete_message(
                    QueueUrl=request_queue_url,
                    ReceiptHandle=message['ReceiptHandle']
                )

if __name__ == '__main__':
    request_queue_name = 'feedback-request-queue'
    response_queue_name = 'feedback-response-queue'
    listen_for_messages(request_queue_name, response_queue_name)
