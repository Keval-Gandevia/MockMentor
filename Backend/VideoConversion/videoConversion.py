import os
import subprocess
import boto3
import json

region_name = os.getenv('AWS_DEFAULT_REGION')
aws_access_key_id = os.getenv('AWS_ACCESS_KEY_ID')
aws_secret_access_key = os.getenv('AWS_SECRET_ACCESS_KEY')
aws_session_token = os.getenv('AWS_SESSION_TOKEN')

s3 = boto3.client('s3', region_name=region_name, aws_access_key_id=aws_access_key_id, aws_secret_access_key=aws_secret_access_key, aws_session_token=aws_session_token)
sqs = boto3.client('sqs', region_name=region_name, aws_access_key_id=aws_access_key_id, aws_secret_access_key=aws_secret_access_key, aws_session_token=aws_session_token)

def convert_video_format(bucket_name, video_url, question_id):
    print(f"Starting video conversion for {video_url} with question ID {question_id}")
    object_name = video_url.split('/')[-1]
    video_file_name = object_name
    converted_video_file_name = f"{question_id}_video.mp4"
    
    s3.download_file(bucket_name, object_name, video_file_name)

    command = [
        'ffmpeg', '-i', video_file_name, '-c:v', 'libx264', '-c:a', 'aac',
        '-strict', 'experimental', converted_video_file_name
    ]
    subprocess.run(command, check=True)
    print(f"Uploading converted video to S3 bucket {bucket_name}")
    s3.upload_file(converted_video_file_name, bucket_name, converted_video_file_name)

    print(f"Removing local files: {video_file_name}, {converted_video_file_name}")
    os.remove(video_file_name)
    os.remove(converted_video_file_name)
    
    return f"https://{bucket_name}.s3.amazonaws.com/{converted_video_file_name}"

def process_video_conversion(request_body):
    body = json.loads(request_body)
    question_id = body.get('questionId')
    video_url = body.get('videoUrl')
    messageType = body.get('messageType')
    bucket_name = 'mock-mentor-bucket'
    
    converted_video_url = convert_video_format(bucket_name, video_url, question_id)
    print(f"Converted video URL: {converted_video_url}")
    
    response = {
        'questionId': question_id,
        'videoUrl': converted_video_url,
        'messageType': messageType
    }
    
    return response

def create_queue_if_not_exists(queue_name):
    response = sqs.list_queues(QueueNamePrefix=queue_name)
    if 'QueueUrls' in response:
        for url in response['QueueUrls']:
            if queue_name in url:
                return url
    
    response = sqs.create_queue(QueueName=queue_name)
    return response['QueueUrl']

def send_response(response_queue_url, message):
    print(f"Sending response message to queue: {response_queue_url}")
    sqs.send_message(
        QueueUrl=response_queue_url,
        MessageBody=json.dumps(message)
    )

def process_message(message, response_queue_url):
    body = json.loads(message['Body'])
    response = process_video_conversion(json.dumps(body))
    send_response(response_queue_url, response)

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
    print("video conversion is running....")
    request_queue_name = 'video-conversion-request-queue'
    response_queue_name = 'video-conversion-response-queue'
    listen_for_messages(request_queue_name, response_queue_name)
