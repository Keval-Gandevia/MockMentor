import os
import uuid
import subprocess
import speech_recognition as sr
import boto3
import json

region_name = os.getenv('AWS_DEFAULT_REGION')
aws_access_key_id = os.getenv('AWS_ACCESS_KEY_ID')
aws_secret_access_key = os.getenv('AWS_SECRET_ACCESS_KEY')
aws_session_token = os.getenv('AWS_SESSION_TOKEN')

s3 = boto3.client('s3', region_name=region_name, aws_access_key_id=aws_access_key_id, aws_secret_access_key=aws_secret_access_key, aws_session_token=aws_session_token)
sqs = boto3.client('sqs', region_name=region_name, aws_access_key_id=aws_access_key_id, aws_secret_access_key=aws_secret_access_key, aws_session_token=aws_session_token)

def extract_speech_from_video(bucket_name, video_url):

    object_name = video_url.split('/')[-1]
    video_file_name = object_name
    s3.download_file(bucket_name, object_name, video_file_name)

    audio_file_name = str(uuid.uuid4()) + ".wav"
    command = [
        'ffmpeg', '-i', video_file_name, '-vn', '-acodec', 'pcm_s16le',
        '-ar', '44100', '-ac', '2', audio_file_name
    ]
    subprocess.run(command, check=True)

    r = sr.Recognizer()
    with sr.AudioFile(audio_file_name) as source:
        audio_text = r.record(source)
    text = r.recognize_google(audio_text, language='en-US')

    os.remove(video_file_name)
    os.remove(audio_file_name)

    return text

def transcribe_video(request_body):
    body = json.loads(request_body)
    question_id = body.get('questionId')
    video_url = body.get('videoUrl')
    bucket_name = 'mock-mentor-bucket'

    transcribed_text = extract_speech_from_video(bucket_name, video_url)
    print(transcribed_text)

    response = {
        'questionId': question_id,
        'answerText': transcribed_text
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
    sqs.send_message(
        QueueUrl=response_queue_url,
        MessageBody=json.dumps(message)
    )

def process_message(message, response_queue_url):
    body = json.loads(message['Body'])
    response = transcribe_video(json.dumps(body))
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
    request_queue_name = 'answer-request-queue'
    response_queue_name = 'answer-response-queue'
    listen_for_messages(request_queue_name, response_queue_name)
